import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output, computed } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-operating-hours-day-accordion',
  imports: [ReactiveFormsModule],
  templateUrl: './operating-hours-day-accordion.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OperatingHoursDayAccordionComponent {
  @Input({ required: true }) dayForm!: FormGroup;
  @Input({ required: true }) expanded = false;
  @Output() toggled = new EventEmitter<void>();
  @Output() addResting = new EventEmitter<void>();
  @Output() removeResting = new EventEmitter<void>();
  @Output() copyToAll = new EventEmitter<void>();
  @Output() copyToWeekdays = new EventEmitter<void>();

  readonly summary = computed(() => {
    const isClosed = this.dayForm?.get('isClosed')?.value;
    if (isClosed) {
      return 'Closed';
    }

    const open = this.dayForm?.get('openTime')?.value;
    const close = this.dayForm?.get('closeTime')?.value;
    const restStart = this.dayForm?.get('restingStartTime')?.value;
    const restEnd = this.dayForm?.get('restingEndTime')?.value;

    if (restStart && restEnd) {
      return `${open} - ${restStart} · ${restEnd} - ${close}`;
    }

    return `${open ?? '--:--'} - ${close ?? '--:--'}`;
  });

  get hasRestingTime(): boolean {
    return !!this.dayForm.get('restingStartTime')?.value || !!this.dayForm.get('restingEndTime')?.value;
  }
}
