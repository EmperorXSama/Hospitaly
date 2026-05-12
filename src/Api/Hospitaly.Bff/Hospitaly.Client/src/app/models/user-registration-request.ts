export interface UserRegistrationRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  sex: string;
  dateOfBirth: string;
  bloodType: string | null;
}
