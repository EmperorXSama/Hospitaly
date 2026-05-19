import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-clinic-scheduling-placeholder-page',
  template: `
    <div class="p-6 lg:p-8">
      <div class="rounded-xl border border-hairline bg-surface-card p-6">
        <p class="text-xs uppercase tracking-[1.4px] text-muted">Clinic / Scheduling</p>
        <h1 class="text-2xl font-bold text-on-dark mt-2">Scheduling (Coming Soon)</h1>
        <p class="text-body mt-3">Appointment slots and special events will be managed here later.</p>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ClinicSchedulingPlaceholderPageComponent {}
