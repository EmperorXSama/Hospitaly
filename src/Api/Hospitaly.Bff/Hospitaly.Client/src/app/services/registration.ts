import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/api-response';
import { UserRegistrationRequest } from '../models/user-registration-request';

@Injectable({ providedIn: 'root' })
export class RegistrationService {
  private readonly bffUserUrl = 'https://localhost:7214/bff/user';

  constructor(private readonly http: HttpClient) {}

  register(payload: UserRegistrationRequest): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(`${this.bffUserUrl}/register`, payload);
  }
}
