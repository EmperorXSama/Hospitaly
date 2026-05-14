import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { finalize } from 'rxjs';
import { ClinicDay, ClinicOperatingHoursUpdateRequest, ClinicOperatingHoursViewModel } from '../../models/clinic-operating-hours';
import { ClinicOperatingHoursService } from '../../services/clinic-operating-hours';
import { NotificationService } from '../../services/notification.service';
import { OperatingHoursCompactManagerComponent } from './operating-hours-compact-manager.component';
import { OperatingHoursSummaryComponent } from './operating-hours-summary.component';
import { SchedulingFutureRulesComponent } from './scheduling-future-rules.component';
import { SchedulingWorkspacePlaceholderComponent } from './scheduling-workspace-placeholder.component';

type DayFormGroup = FormGroup<{
  day: FormControl<ClinicDay>;
  isClosed: FormControl<boolean>;
  openTime: FormControl<string | null>;
  closeTime: FormControl<string | null>;
  restingStartTime: FormControl<string | null>;
  restingEndTime: FormControl<string | null>;
}>;

const DEFAULT_WEEK_SCHEDULE: ClinicOperatingHoursViewModel[] = [
  { day: 'Monday', isClosed: false, openTime: '09:00', closeTime: '17:00', restingStartTime: '13:30', restingEndTime: '14:30' },
  { day: 'Tuesday', isClosed: false, openTime: '09:00', closeTime: '17:00', restingStartTime: null, restingEndTime: null },
  { day: 'Wednesday', isClosed: false, openTime: '09:00', closeTime: '17:00', restingStartTime: null, restingEndTime: null },
  { day: 'Thursday', isClosed: false, openTime: '09:00', closeTime: '17:00', restingStartTime: null, restingEndTime: null },
  { day: 'Friday', isClosed: false, openTime: '09:00', closeTime: '16:00', restingStartTime: '12:30', restingEndTime: '13:30' },
  { day: 'Saturday', isClosed: true, openTime: null, closeTime: null, restingStartTime: null, restingEndTime: null },
  { day: 'Sunday', isClosed: true, openTime: null, closeTime: null, restingStartTime: null, restingEndTime: null },
];

@Component({
  selector: 'app-clinic-operating-hours-scheduling-page',
  imports: [
    ReactiveFormsModule,
    OperatingHoursCompactManagerComponent,
    OperatingHoursSummaryComponent,
    SchedulingFutureRulesComponent,
    SchedulingWorkspacePlaceholderComponent,
  ],
  templateUrl: './clinic-operating-hours-page.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ClinicOperatingHoursSchedulingPageComponent {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly operatingHoursService = inject(ClinicOperatingHoursService);
  private readonly notificationService = inject(NotificationService);

  readonly clinicId = this.route.snapshot.paramMap.get('clinicId') ?? '';
  readonly isLoading = signal(true);
  readonly isSaving = signal(false);
  readonly hasSchedule = signal(false);
  readonly saveError = signal<string | null>(null);
  readonly expandedIndex = signal(0);
  private readonly formVersion = signal(0);

  private readonly baseline = signal<ClinicOperatingHoursViewModel[]>([]);

  readonly form = this.fb.group({
    days: this.fb.array<DayFormGroup>([]),
  });

  readonly hasUnsavedChanges = computed(() => {
    this.formVersion();
    return this.form.dirty && JSON.stringify(this.daysValue()) !== JSON.stringify(this.baseline());
  });
  readonly hasValidationErrors = computed(() => {
    this.formVersion();
    return this.form.invalid;
  });
  readonly openDays = computed(() => this.daysValue().filter((x) => !x.isClosed).length);
  readonly closedDays = computed(() => this.daysValue().filter((x) => x.isClosed).length);
  readonly restingDays = computed(() => this.daysValue().filter((x) => !!x.restingStartTime && !!x.restingEndTime).length);
  readonly earliestOpening = computed(() => this.daysValue().filter((x) => x.openTime).map((x) => x.openTime as string).sort()[0] ?? null);
  readonly latestClosing = computed(() => {
    const values = this.daysValue().filter((x) => x.closeTime).map((x) => x.closeTime as string).sort();
    return values[values.length - 1] ?? null;
  });
  readonly todayStatus = computed(() => {
    const idx = (new Date().getDay() + 6) % 7;
    const day = this.daysValue()[idx];
    if (!day || day.isClosed) {
      return 'Closed today';
    }
    if (day.restingStartTime && day.restingEndTime) {
      return `Open with break ${day.restingStartTime}-${day.restingEndTime}`;
    }
    return `Open until ${day.closeTime}`;
  });

  constructor() {
    this.loadOperatingHours();
    this.form.valueChanges.subscribe(() => this.formVersion.update((v) => v + 1));
  }

  get days(): FormArray<DayFormGroup> {
    return this.form.controls.days;
  }

  createWeeklySchedule(): void {
    this.hasSchedule.set(true);
    this.patchDays(DEFAULT_WEEK_SCHEDULE);
    this.form.markAsDirty();
    this.formVersion.update((value) => value + 1);
  }

  toggleDay(index: number): void {
    this.expandedIndex.update((value) => (value === index ? -1 : index));
  }

  addResting(index: number): void {
    const day = this.days.at(index);
    if (!day || day.controls.isClosed.value) {
      return;
    }
    day.patchValue({ restingStartTime: '13:00', restingEndTime: '14:00' });
    day.markAsDirty();
    day.updateValueAndValidity();
  }

  removeResting(index: number): void {
    const day = this.days.at(index);
    if (!day) {
      return;
    }
    day.patchValue({ restingStartTime: null, restingEndTime: null });
    day.markAsDirty();
    day.updateValueAndValidity();
  }

  copyDayToAll(index: number): void {
    const source = this.days.at(index);
    if (!source) {
      return;
    }
    this.copyFromSource(source, [0, 1, 2, 3, 4, 5, 6]);
  }

  copyDayToWeekdays(index: number): void {
    const source = this.days.at(index);
    if (!source) {
      return;
    }
    this.copyFromSource(source, [0, 1, 2, 3, 4]);
  }

  resetChanges(): void {
    this.saveError.set(null);
    this.patchDays(this.baseline());
  }

  saveChanges(): void {
    this.saveError.set(null);
    this.form.markAllAsTouched();
    this.days.controls.forEach((control) => control.updateValueAndValidity());

    if (this.form.invalid) {
      this.saveError.set('Please fix validation errors before saving.');
      return;
    }

    this.isSaving.set(true);
    this.operatingHoursService
      .saveOperatingHours(this.clinicId, this.daysValue())
      .pipe(finalize(() => this.isSaving.set(false)))
      .subscribe({
        next: () => {
          this.notificationService.showSuccess('Clinic schedule saved successfully.');
          this.loadOperatingHours();
        },
        error: () => this.saveError.set('Unable to save right now. Please try again.'),
      });
  }

  private copyFromSource(source: DayFormGroup, indexes: number[]): void {
    for (const index of indexes) {
      const target = this.days.at(index);
      if (!target || target === source) {
        continue;
      }
      target.patchValue({
        isClosed: source.controls.isClosed.value,
        openTime: source.controls.isClosed.value ? null : source.controls.openTime.value,
        closeTime: source.controls.isClosed.value ? null : source.controls.closeTime.value,
        restingStartTime: source.controls.isClosed.value ? null : source.controls.restingStartTime.value,
        restingEndTime: source.controls.isClosed.value ? null : source.controls.restingEndTime.value,
      });
      target.markAsDirty();
      target.updateValueAndValidity();
    }
    this.form.markAsDirty();
    this.formVersion.update((value) => value + 1);
  }

  private loadOperatingHours(): void {
    if (!this.clinicId) {
      this.isLoading.set(false);
      this.hasSchedule.set(false);
      this.patchDays([]);
      return;
    }

    this.isLoading.set(true);
    this.operatingHoursService.getOperatingHours(this.clinicId).subscribe({
      next: (days) => {
        this.hasSchedule.set(days.length > 0);
        this.baseline.set(days);
        this.patchDays(days);
        this.isLoading.set(false);
      },
      error: () => {
        this.hasSchedule.set(false);
        this.patchDays([]);
        this.isLoading.set(false);
      },
    });
  }

  private patchDays(days: ClinicOperatingHoursViewModel[]): void {
    this.days.clear();
    for (const day of days) {
      this.days.push(this.createDayForm(day));
    }
    this.form.markAsPristine();
    this.formVersion.update((value) => value + 1);
  }

  private createDayForm(day: ClinicOperatingHoursViewModel): DayFormGroup {
    const form = this.fb.group(
      {
        day: this.fb.nonNullable.control<ClinicDay>(day.day),
        isClosed: this.fb.nonNullable.control(day.isClosed),
        openTime: this.fb.control<string | null>(day.openTime),
        closeTime: this.fb.control<string | null>(day.closeTime),
        restingStartTime: this.fb.control<string | null>(day.restingStartTime),
        restingEndTime: this.fb.control<string | null>(day.restingEndTime),
      },
      { validators: [this.dayValidator()] },
    );

    form.controls.isClosed.valueChanges.subscribe((isClosed) => {
      if (isClosed) {
        form.patchValue({ openTime: null, closeTime: null, restingStartTime: null, restingEndTime: null }, { emitEvent: false });
      }
      form.updateValueAndValidity({ emitEvent: false });
    });

    return form;
  }

  private dayValidator(): ValidatorFn {
    return (control): ValidationErrors | null => {
      const form = control as DayFormGroup;
      const isClosed = form.controls.isClosed.value;
      const open = form.controls.openTime.value;
      const close = form.controls.closeTime.value;
      const restStart = form.controls.restingStartTime.value;
      const restEnd = form.controls.restingEndTime.value;

      if (isClosed) {
        if (open || close || restStart || restEnd) {
          return { closedMustBeNull: true };
        }
        return null;
      }

      if (!open || !close) {
        return { missingTimes: true };
      }
      if (close <= open) {
        return { closeBeforeOpen: true };
      }

      const hasRestStart = !!restStart;
      const hasRestEnd = !!restEnd;
      if (hasRestStart !== hasRestEnd) {
        return { restingIncomplete: true };
      }
      if (restStart && restEnd) {
        if (restEnd <= restStart) {
          return { restingOrder: true };
        }
        if (restStart <= open || restEnd >= close) {
          return { restingOutsideHours: true };
        }
      }

      return null;
    };
  }

  private daysValue(): ClinicOperatingHoursUpdateRequest[] {
    return this.days.controls.map((day) => ({
      day: day.controls.day.value,
      isClosed: day.controls.isClosed.value,
      openTime: day.controls.isClosed.value ? null : day.controls.openTime.value,
      closeTime: day.controls.isClosed.value ? null : day.controls.closeTime.value,
      restingStartTime: day.controls.isClosed.value ? null : day.controls.restingStartTime.value,
      restingEndTime: day.controls.isClosed.value ? null : day.controls.restingEndTime.value,
    }));
  }
}


