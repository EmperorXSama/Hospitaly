import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ClientUserData } from '../models/client-user-data';
import { UserProfile } from '../models/user-profile';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class UserDataService {
  private readonly http = inject(HttpClient);
  private readonly apiService = inject(ApiService);
  private readonly bffUserUrl = 'https://localhost:7214/bff/user';

  getMe(): Observable<ClientUserData> {
    return this.http.get<ClientUserData>(`${this.bffUserUrl}/me`);
  }

  getProfileData(): Observable<UserProfile> {
    return this.apiService.get<UserProfile>('https://localhost:7214/api/users/profile');
  }
}
