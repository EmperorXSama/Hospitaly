import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { ApiResponse } from '../models/api-response';

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

  constructor(private readonly http: HttpClient) {}

  createDoctor(): Observable<string> {
    return this.http
      .post<ApiResponse<string>>(`https://localhost:7214/api/doctors`, {})
      .pipe(map((r) => r.data));
  }

  createClinic(request: CreateClinicRequest): Observable<string> {
    return this.http
      .post<ApiResponse<string>>(`https://localhost:7214/api/clinics`, request)
      .pipe(map((r) => r.data));
  }

  completeOnboarding(): Observable<void> {
    return this.http.post<void>(`https://localhost:7214/bff/user/onboarding/complete`, {});
  }
}
