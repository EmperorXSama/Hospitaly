import { Component, ChangeDetectionStrategy, input, output } from '@angular/core';

@Component({
  selector: 'app-config-modal',
  imports: [],
  templateUrl: './config-modal.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ConfigModal {
  readonly userId = input.required<string>();
  readonly close = output<void>();
}
