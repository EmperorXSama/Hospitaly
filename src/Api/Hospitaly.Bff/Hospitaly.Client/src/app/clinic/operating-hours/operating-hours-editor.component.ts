import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { FormArray, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { OperatingHoursDayCardComponent } from './operating-hours-day-card.component';

@Component({
  selector: 'app-operating-hours-editor',
  imports: [ReactiveFormsModule, OperatingHoursDayCardComponent],
  templateUrl: './operating-hours-editor.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OperatingHoursEditorComponent {
  @Input({ required: true }) daysFormArray!: FormArray<FormGroup>;
  @Output() copyMondayToWeekdays = new EventEmitter<void>();
}
