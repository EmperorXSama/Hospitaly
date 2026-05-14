import { ChangeDetectionStrategy, Component, computed, input, output } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ClinicOwnershipResponse } from '../../../models/clinic-ownership';

@Component({
  selector: 'app-owner-card',
  imports: [DatePipe],
  templateUrl: './owner-card.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OwnerCardComponent {
  readonly ownership = input.required<ClinicOwnershipResponse>();
  readonly transfer = output<ClinicOwnershipResponse>();

  readonly initials = computed(() => {
    const owner = this.ownership().owner;
    if (!owner) return '--';
    return `${owner.firstName.charAt(0)}${owner.lastName.charAt(0)}`.toUpperCase();
  });

  readonly fullName = computed(() => {
    const owner = this.ownership().owner;
    if (!owner) return 'Unknown Owner';
    return `${owner.firstName} ${owner.lastName}`;
  });

  readonly joinedYear = computed(() => {
    const start = this.ownership().effectiveStart;
    if (!start) return '';
    return new Date(start).getFullYear().toString();
  });

  readonly isActive = computed(() => this.ownership().status === 'Active');
}
