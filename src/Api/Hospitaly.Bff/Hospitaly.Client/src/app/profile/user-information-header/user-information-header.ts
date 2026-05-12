import { Component, ChangeDetectionStrategy, computed, input } from '@angular/core';

export interface UserInfo {
  fullName: string;
  email: string;
  sex: string;
  age: number;
  bloodType: string;
  registeredDate: string;
  roles: readonly string[];
}

@Component({
  selector: 'app-user-information-header',
  standalone: true,
  imports: [],
  templateUrl: './user-information-header.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserInformationHeaderComponent {
  readonly user = input.required<UserInfo>();

  readonly initials = computed(() => {
    const parts = this.user().fullName.split(' ');
    if (parts.length >= 2) {
      return parts[0][0] + parts[parts.length - 1][0];
    }
    return parts[0]?.[0] ?? '?';
  });
}
