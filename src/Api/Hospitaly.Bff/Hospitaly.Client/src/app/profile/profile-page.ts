import { Component, ChangeDetectionStrategy, inject, computed, signal, effect } from '@angular/core';
import { AuthService } from '../services/auth';
import { UserDataService } from '../services/user-data';
import { UserProfile } from '../models/user-profile';
import { UserInformationHeaderComponent, UserInfo } from './user-information-header/user-information-header';

@Component({
  selector: 'app-profile-page',
  imports: [UserInformationHeaderComponent],
  templateUrl: './profile-page.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfilePage {
  private readonly authService = inject(AuthService);
  private readonly userDataService = inject(UserDataService);

  profile = this.authService.profile;
  user = this.authService.user;

  readonly profileData = signal<UserProfile | null>(null);

  constructor() {
    this.userDataService.getProfileData().subscribe({
      next: (data) => this.profileData.set(data),
    });
  }

  readonly userInfo = computed<UserInfo>(() => {
    const p = this.profile();
    const pd = this.profileData();
    return {
      fullName: p?.userName ?? this.user()?.['name'] ?? 'User',
      email: p?.email ?? this.user()?.['email'] ?? '—',
      sex: pd?.sex ?? '—',
      age: pd?.dateOfBirth ? this.computeAge(pd.dateOfBirth) : 0,
      bloodType: pd?.bloodType ?? '—',
      registeredDate: pd?.createdOnUtc ? this.formatDate(pd.createdOnUtc) : '—',
      roles: p?.roles?.length ? p.roles : ['Member'],
    };
  });

  private computeAge(dateOfBirth: string): number {
    const birth = new Date(dateOfBirth);
    const today = new Date();
    let age = today.getFullYear() - birth.getFullYear();
    const monthDiff = today.getMonth() - birth.getMonth();
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birth.getDate())) {
      age--;
    }
    return age;
  }

  private formatDate(iso: string): string {
    const date = new Date(iso);
    return date.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    });
  }
}
