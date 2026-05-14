import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { FormArray, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { OperatingHoursDayAccordionComponent } from './operating-hours-day-accordion.component';

@Component({
  selector: 'app-operating-hours-compact-manager',
  imports: [ReactiveFormsModule, OperatingHoursDayAccordionComponent],
  templateUrl: './operating-hours-compact-manager.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OperatingHoursCompactManagerComponent {
  @Input({ required: true }) daysFormArray!: FormArray<FormGroup>;
  @Input({ required: true }) expandedIndex = 0;

  @Output() toggleDay = new EventEmitter<number>();
  @Output() addResting = new EventEmitter<number>();
  @Output() removeResting = new EventEmitter<number>();
  @Output() copyToAll = new EventEmitter<number>();
  @Output() copyToWeekdays = new EventEmitter<number>();
}
