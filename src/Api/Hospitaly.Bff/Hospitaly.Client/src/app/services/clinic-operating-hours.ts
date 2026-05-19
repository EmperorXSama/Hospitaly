import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { ApiResponse } from '../models/api-response';
import { ClinicOperatingHoursUpdateRequest, ClinicOperatingHoursViewModel } from '../models/clinic-operating-hours';
import { ApiService } from './api.service';

interface ClinicOperatingHoursApiRow {
  day: number;
  hoursActive: boolean;
  openTime: string | null;
  closeTime: string | null;
  restingTimeActive: boolean;
  restingStartTime: string | null;
  restingEndTime: string | null;
}

interface ClinicOperatingHoursApiResponse {
  clinicId: string;
  operatingHours: ClinicOperatingHoursApiRow[];
}

interface SetClinicOperatingHoursBody {
  clinicId: string;
  userId: string;
  operatingHours: {
    day: number;
    isClosed: boolean;
    startTime: string | null;
    endTime: string | null;
    restingStartsAt: string | null;
    restingEndsAt: string | null;
  }[];
}

@Injectable({ providedIn: 'root' })
export class ClinicOperatingHoursService {
  private readonly apiService = inject(ApiService);
  private readonly http = inject(HttpClient);
  private readonly bffApiBaseUrl = 'https://localhost:7214/api';

  getOperatingHours(clinicId: string): Observable<ClinicOperatingHoursViewModel[]> {
    return this.apiService
      .get<ClinicOperatingHoursApiResponse>(`${this.bffApiBaseUrl}/clinics/getClinicOperatingHours?clinicId=${clinicId}`)
      .pipe(map((response) => (response.operatingHours ?? []).map((row) => this.fromApi(row))));
  }

  saveOperatingHours(clinicId: string, request: ClinicOperatingHoursUpdateRequest[]): Observable<void> {
    const body: SetClinicOperatingHoursBody = {
      clinicId,
      userId: '00000000-0000-0000-0000-000000000000',
      operatingHours: request.map((day) => ({
        day: this.toDayOfWeek(day.day),
        isClosed: day.isClosed,
        startTime: this.toTimeSpan(day.openTime),
        endTime: this.toTimeSpan(day.closeTime),
        restingStartsAt: this.toTimeSpan(day.restingStartTime),
        restingEndsAt: this.toTimeSpan(day.restingEndTime),
      })),
    };

    return this.http.post<ApiResponse<unknown>>(`${this.bffApiBaseUrl}/clinics/setClinicOperatingHours`, body).pipe(
      map((envelope) => {
        if (!envelope.isSuccess) {
          throw new Error(envelope.error?.message ?? 'Unable to save operating hours.');
        }
      }),
    );
  }

  private toDayOfWeek(day: ClinicOperatingHoursViewModel['day']): number {
    const map: Record<ClinicOperatingHoursViewModel['day'], number> = {
      Sunday: 0,
      Monday: 1,
      Tuesday: 2,
      Wednesday: 3,
      Thursday: 4,
      Friday: 5,
      Saturday: 6,
    };

    return map[day];
  }
  private normalizeDay(day: string | number): ClinicOperatingHoursViewModel['day'] {
    if (typeof day === 'number') {
      const dayMap: Record<number, ClinicOperatingHoursViewModel['day']> = {
        0: 'Sunday',
        1: 'Monday',
        2: 'Tuesday',
        3: 'Wednesday',
        4: 'Thursday',
        5: 'Friday',
        6: 'Saturday',
      };
      return dayMap[day] ?? 'Monday';
    }

    const normalized = day.trim().toLowerCase();
    const stringMap: Record<string, ClinicOperatingHoursViewModel['day']> = {
      monday: 'Monday',
      tuesday: 'Tuesday',
      wednesday: 'Wednesday',
      thursday: 'Thursday',
      friday: 'Friday',
      saturday: 'Saturday',
      sunday: 'Sunday',
    };

    return stringMap[normalized] ?? 'Monday';
  }
  private fromApi(row: ClinicOperatingHoursApiRow): ClinicOperatingHoursViewModel {
    const openTime = row.hoursActive ? this.toInputTime(row.openTime) : null;
    const closeTime = row.hoursActive ? this.toInputTime(row.closeTime) : null;

    return {
      day: this.normalizeDay(row.day),
      isClosed: !row.hoursActive,
      openTime,
      closeTime,
      restingStartTime: row.restingTimeActive ? this.toInputTime(row.restingStartTime) : null,
      restingEndTime: row.restingTimeActive ? this.toInputTime(row.restingEndTime) : null,
    };
  }

  private toInputTime(value: string | null): string | null {
    if (!value) {
      return null;
    }

    const isoMatch = value.match(/T(\d{2}:\d{2})/);
    if (isoMatch?.[1]) {
      return isoMatch[1];
    }

    const timeOnlyMatch = value.match(/^(\d{2}:\d{2})/);
    if (timeOnlyMatch?.[1]) {
      return timeOnlyMatch[1];
    }

    const parsed = new Date(value);
    if (!Number.isNaN(parsed.getTime())) {
      return `${String(parsed.getHours()).padStart(2, '0')}:${String(parsed.getMinutes()).padStart(2, '0')}`;
    }

    return null;
  }

  private toTimeSpan(value: string | null): string | null {
    return value ? `${value}:00` : null;
  }
}





