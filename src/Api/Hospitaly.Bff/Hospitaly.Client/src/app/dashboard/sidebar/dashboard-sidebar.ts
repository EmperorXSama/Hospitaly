import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../services/auth';

export interface NavItem {
  label: string;
  path: string;
  icon: string;
  exact?: boolean;
}

@Component({
  selector: 'app-dashboard-sidebar',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './dashboard-sidebar.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardSidebar {
  private readonly authService = inject(AuthService);
  profile = this.authService.profile;
  isAuthenticated = this.authService.isAuthenticated;

  readonly mainNav: NavItem[] = [
    { label: 'Dashboard', path: '/dashboard', icon: 'dashboard', exact: true },
    { label: 'Appointments', path: '/appointments', icon: 'calendar' },
    { label: 'Patients', path: '/patients', icon: 'users' },
    { label: 'Doctors', path: '/doctors', icon: 'activity' },
    { label: 'Clinics', path: '/clinics', icon: 'building' },
    { label: 'Reports', path: '/reports', icon: 'chart' },
    { label: 'Profile', path: '/dashboard/profile', icon: 'user', exact: true },
    { label: 'Settings', path: '/settings', icon: 'settings' },
  ];

  readonly bottomNav: NavItem[] = [
    { label: 'Help & Information', path: '', icon: 'help' },
  ];

  logout(): void {
    this.authService.logout();
  }
}
