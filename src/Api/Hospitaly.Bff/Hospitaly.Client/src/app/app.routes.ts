import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./landing/landing').then((m) => m.Landing),
  },
  {
    path: 'register',
    loadComponent: () => import('./register/register-page').then((m) => m.RegisterPage),
  },
  {
    path: 'onboarding',
    loadComponent: () => import('./onboarding/onboarding-page').then((m) => m.OnboardingPage),
    canActivate: [authGuard],
  },
  {
    path: 'profile',
    redirectTo: 'dashboard/profile',
    pathMatch: 'full',
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./dashboard/dashboard-shell/dashboard-shell').then((m) => m.DashboardShell),
    canActivate: [authGuard],
    children: [
      {
        path: '',
        loadComponent: () => import('./dashboard/dashboard-page').then((m) => m.DashboardPage),
      },
      {
        path: 'profile',
        loadComponent: () => import('./profile/profile-page').then((m) => m.ProfilePage),
      },
      {
        path: 'clinics',
        canActivate: [authGuard],
        data: { permission: 'clinics:read' },
        loadComponent: () => import('./clinic/clinics/clinics-page').then((m) => m.ClinicsPage),
      },
      {
        path: 'clinics/:clinicId/schedule',
        canActivate: [authGuard],
        data: { permission: 'clinics:update' },
        loadComponent: () =>
          import('./clinic/operating-hours/clinic-operating-hours-page').then((m) => m.ClinicOperatingHoursSchedulingPageComponent),
      },
      {
        path: 'clinics/:clinicId/ownership',
        canActivate: [authGuard],
        data: { permission: 'clinics:read' },
        loadComponent: () =>
          import('./clinic/ownership/clinic-ownership-page').then((m) => m.ClinicOwnershipPage),
      },
      {
        path: 'clinic/scheduling',
        redirectTo: 'clinics',
        pathMatch: 'full',
      },
      {
        path: 'clinic/operating-hours',
        redirectTo: 'clinics',
        pathMatch: 'full',
      },
    ],
  },
  { path: '**', redirectTo: '' },
];
