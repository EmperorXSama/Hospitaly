import { Component, ChangeDetectionStrategy, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { OnboardingService, CreateClinicRequest } from '../services/onboarding';
import { AuthService } from '../services/auth';
import { REGIONS, getCitiesForRegion } from '../shared/data/morocco-regions';

@Component({
  selector: 'app-onboarding-page',
  imports: [FormsModule],
  templateUrl: './onboarding-page.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OnboardingPage {
  private readonly onboardingService = inject(OnboardingService);
  private readonly authService = inject(AuthService);
  readonly router = inject(Router);

  step = signal(1);
  selectedRole = signal<'doctor' | 'clinic' | 'none' | null>(null);
  isLoading = signal(false);
  errorMessage = signal<string | null>(null);

  readonly REGIONS = REGIONS;

  clinicName = '';
  clinicDescription = '';
  clinicStreet = '';
  clinicRegion = signal('');
  clinicCity = signal('');
  clinicPostalCode = '';
  clinicPhone = '';
  clinicEmail = '';

  availableCities = computed(() => getCitiesForRegion(this.clinicRegion()));

  readonly COUNTRY = 'Morocco';

  selectRole(role: 'doctor' | 'clinic' | 'none'): void {
    this.selectedRole.set(role);
    this.step.set(2);
  }

  back(): void {
    this.step.set(1);
    this.errorMessage.set(null);
  }

  submitDoctor(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.onboardingService.createDoctor().subscribe({
      next: () => this.complete(),
      error: () => {
        this.errorMessage.set('Failed to create doctor profile. Please try again.');
        this.isLoading.set(false);
      },
    });
  }

  submitClinic(): void {
    if (!this.clinicName || !this.clinicStreet || !this.clinicRegion() || !this.clinicCity()) {
      this.errorMessage.set('Please fill in all required fields.');
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);

    const request: CreateClinicRequest = {
      name: this.clinicName,
      description: this.clinicDescription,
      street: this.clinicStreet,
      city: this.clinicCity(),
      region: this.clinicRegion(),
      postalCode: this.clinicPostalCode || null,
      country: this.COUNTRY,
      phone: this.clinicPhone || null,
      email: this.clinicEmail || null,
    };

    this.onboardingService.createClinic(request).subscribe({
      next: () => this.complete(),
      error: () => {
        this.errorMessage.set('Failed to create clinic. Please try again.');
        this.isLoading.set(false);
      },
    });
  }

  submitNone(): void {
    this.complete();
  }

  private complete(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.onboardingService.completeOnboarding().subscribe({
      next: () => {
        this.authService.profile.set(null);
        this.authService.checkSession().subscribe(() => {
          this.isLoading.set(false);
          this.router.navigate(['/dashboard']);
        });
      },
      error: () => {
        this.authService.profile.set(null);
        this.isLoading.set(false);
        this.errorMessage.set('Failed to complete setup. Please try again.');
      },
    });
  }
}
