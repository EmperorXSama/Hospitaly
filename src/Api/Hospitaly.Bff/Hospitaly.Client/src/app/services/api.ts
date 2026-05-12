// src/app/services/api.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private bffUrl = 'https://localhost:7214/bff/auth';

  constructor(private http: HttpClient) {}
  get<T = unknown>(endpoint: string): Observable<T> {
    return this.http.get<T>(endpoint);
  }
  init(): Observable<unknown> {
    return this.http.get<unknown>(`${this.bffUrl}/protected-data`, { withCredentials: true });
  }
}
