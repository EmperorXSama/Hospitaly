import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-operating-hours-day-card',
  imports: [ReactiveFormsModule],
  templateUrl: './operating-hours-day-card.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OperatingHoursDayCardComponent {
  readonly dayForm = input.required<FormGroup>();
  readonly canCopy = input<boolean>(true);

  readonly statusLabel = computed(() => {
    const form = this.dayForm();
    if (form.get('isClosed')?.value) {
      return 'Closed';
    }

    if (form.get('isResting')?.value) {
      return 'Resting';
    }

    return 'Open';
  });

  readonly statusClasses = computed(() => {
    const status = this.statusLabel();

    if (status === 'Closed') {
      return 'text-error bg-error/10 border-error/30';
    }

    if (status === 'Resting') {
      return 'text-warning bg-warning/10 border-warning/30';
    }

    return 'text-success bg-success/10 border-success/30';
  });
}
