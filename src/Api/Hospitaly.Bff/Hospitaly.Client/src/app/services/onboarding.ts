import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface CreateClinicRequest {
  name: string;
  description: string;
  street: string;
  city: string;
  region: string | null;
  postalCode: string | null;
  country: string;
  phone: string | null;
  email: string | null;
}

@Injectable({ providedIn: 'root' })
export class OnboardingService {
  private readonly http = inject(HttpClient);
  private readonly apiService = inject(ApiService);

  createDoctor(): Observable<string> {
    return this.apiService.post<string>('https://localhost:7214/api/doctors', {});
  }

  createClinic(request: CreateClinicRequest): Observable<string> {
    return this.apiService.post<string>('https://localhost:7214/api/clinics', request);
  }

  completeOnboarding(): Observable<void> {
    return this.http.post<void>(`https://localhost:7214/bff/user/onboarding/complete`, {});
  }
}
