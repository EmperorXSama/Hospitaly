export type ClinicDay =
  | 'Monday'
  | 'Tuesday'
  | 'Wednesday'
  | 'Thursday'
  | 'Friday'
  | 'Saturday'
  | 'Sunday';

export interface ClinicOperatingHoursViewModel {
  day: ClinicDay;
  isClosed: boolean;
  openTime: string | null;
  closeTime: string | null;
  restingStartTime: string | null;
  restingEndTime: string | null;
}

export interface ClinicOperatingHoursUpdateRequest {
  day: ClinicDay;
  isClosed: boolean;
  openTime: string | null;
  closeTime: string | null;
  restingStartTime: string | null;
  restingEndTime: string | null;
}
