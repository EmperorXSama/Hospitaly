import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ClientUserData } from '../models/client-user-data';

@Injectable({ providedIn: 'root' })
export class UserDataService {
  private readonly bffUserUrl = 'https://localhost:7214/bff/user';

  constructor(private readonly http: HttpClient) {}

  getMe(): Observable<ClientUserData> {
    return this.http.get<ClientUserData>(`${this.bffUserUrl}/me`);
  }
}
