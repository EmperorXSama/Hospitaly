import { Component, ChangeDetectionStrategy, Input } from '@angular/core';

@Component({
  selector: 'app-dashboard-activity-item',
  imports: [],
  templateUrl: './dashboard-activity-item.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardActivityItem {
  @Input({ required: true }) icon!: string;
  @Input({ required: true }) title!: string;
  @Input({ required: true }) time!: string;
  @Input({ required: true }) description!: string;
}
