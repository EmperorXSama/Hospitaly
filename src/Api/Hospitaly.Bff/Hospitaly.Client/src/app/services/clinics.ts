import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { UserClinic } from '../models/user-clinic';

@Injectable({ providedIn: 'root' })
export class ClinicsService {
  private readonly api = inject(ApiService);
  private readonly bffApiBaseUrl = 'https://localhost:7214/api';

  getMyClinics(): Observable<UserClinic[]> {
    return this.api.get<UserClinic[]>(`${this.bffApiBaseUrl}/clinics/my`);
  }
}

