import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { ApiResponse } from '../models/api-response';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly http = inject(HttpClient);

  get<T>(url: string): Observable<T> {
    return this.http.get<ApiResponse<T>>(url).pipe(
      map((envelope) => {
        if (!envelope.isSuccess || !envelope.data) {
          throw new Error(envelope.error?.message ?? 'API request failed');
        }
        return envelope.data;
      }),
    );
  }

  post<T>(url: string, body?: unknown): Observable<T> {
    return this.http.post<ApiResponse<T>>(url, body).pipe(
      map((envelope) => {
        if (!envelope.isSuccess || !envelope.data) {
          throw new Error(envelope.error?.message ?? 'API request failed');
        }
        return envelope.data;
      }),
    );
  }
}
