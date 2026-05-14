import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { ClinicsService } from '../../services/clinics';
import { UserClinic } from '../../models/user-clinic';

@Component({
  selector: 'app-clinics-page',
  templateUrl: './clinics-page.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ClinicsPage {
  private readonly clinicsService = inject(ClinicsService);
  private readonly router = inject(Router);

  readonly clinics = signal<UserClinic[]>([]);
  readonly selectedClinic = signal<UserClinic | null>(null);
  readonly isLoading = signal(true);

  constructor() {
    this.clinicsService.getMyClinics().subscribe({
      next: (clinics) => {
        this.clinics.set(clinics);
        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false),
    });
  }

  openClinic(clinic: UserClinic): void {
    this.selectedClinic.set(clinic);
  }

  closeWidget(): void {
    this.selectedClinic.set(null);
  }

  manageSchedule(clinicId: string): void {
    this.router.navigate(['/dashboard/clinics', clinicId, 'schedule']);
  }

  manageOwnership(clinicId: string): void {
    this.router.navigate(['/dashboard/clinics', clinicId, 'ownership']);
  }
}

