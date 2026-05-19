import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { ClinicOwnershipResponse, TransferOwnershipRequest, UserSearchResult } from '../models/clinic-ownership';

@Injectable({ providedIn: 'root' })
export class ClinicOwnershipsService {
  private readonly api = inject(ApiService);
  private readonly bffApiBaseUrl = 'https://localhost:7214/api';

  getOwnerships(clinicId: string): Observable<ClinicOwnershipResponse[]> {
    return this.api.get<ClinicOwnershipResponse[]>(
      `${this.bffApiBaseUrl}/clinics/${clinicId}/ownerships`,
    );
  }

  searchUsers(email: string): Observable<UserSearchResult[]> {
    return this.api.get<UserSearchResult[]>(
      `${this.bffApiBaseUrl}/users/search-by-email?email=${encodeURIComponent(email)}`,
    );
  }

  transferToUser(clinicId: string, request: TransferOwnershipRequest): Observable<void> {
    return this.api.post<void>(
      `${this.bffApiBaseUrl}/clinics/${clinicId}/ownerships/transfer-to-user`,
      request,
    );
  }
}
