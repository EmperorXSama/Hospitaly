// src/app/services/auth.ts
import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, catchError, of, switchMap } from 'rxjs';
import { UserDataService } from './user-data';
import { ClientUserData } from '../models/client-user-data';

export type Claims = Record<string, string>;

@Injectable({ providedIn: 'root' })
export class AuthService {
  private bffUrl = 'https://localhost:7214/bff/auth';
  user = signal<Claims | null>(null);
  profile = signal<ClientUserData | null>(null);
  isAuthenticated = signal(false);

  constructor(
    private readonly http: HttpClient,
    private readonly userDataService: UserDataService,
  ) {}

  checkSession(): Observable<Claims | null> {
    return this.http.get<Claims>(`${this.bffUrl}/check_session`, { withCredentials: true }).pipe(
      switchMap((claims) =>
        this.userDataService.getMe().pipe(
          tap((profile) => {
            this.user.set(claims);
            this.profile.set(profile);
            this.isAuthenticated.set(true);
          }),
          switchMap(() => of(claims)),
          catchError(() => {
            this.user.set(null);
            this.profile.set(null);
            this.isAuthenticated.set(false);
            return of(null);
          }),
        ),
      ),
      catchError(() => {
        this.user.set(null);
        this.profile.set(null);
        this.isAuthenticated.set(false);
        return of(null);
      }),
    );
  }

  login(): void {
    window.location.href = `${this.bffUrl}/login?returnUrl=${encodeURIComponent(window.location.origin + '/dashboard')}`;
  }

  logout(): void {
    this.user.set(null);
    this.profile.set(null);
    this.isAuthenticated.set(false);
    window.location.href = `${this.bffUrl}/logout`;
  }
}
