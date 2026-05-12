import { Component, ChangeDetectionStrategy, Input } from '@angular/core';

@Component({
  selector: 'app-dashboard-metric-card',
  imports: [],
  templateUrl: './dashboard-metric-card.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardMetricCard {
  @Input({ required: true }) value!: string;
  @Input({ required: true }) label!: string;
  @Input({ required: true }) delta!: string;
  @Input({ required: true }) deltaUp!: boolean;
  @Input({ required: true }) icon!: string;
}
