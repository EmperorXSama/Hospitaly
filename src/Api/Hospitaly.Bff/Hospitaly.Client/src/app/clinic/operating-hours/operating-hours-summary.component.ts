import { ChangeDetectionStrategy, Component, input } from '@angular/core';

@Component({
  selector: 'app-operating-hours-summary',
  templateUrl: './operating-hours-summary.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OperatingHoursSummaryComponent {
  readonly openDays = input.required<number>();
  readonly closedDays = input.required<number>();
  readonly restingDays = input.required<number>();
  readonly earliestOpening = input<string | null>(null);
  readonly latestClosing = input<string | null>(null);
  readonly todayStatus = input<string>('Closed today');
}
