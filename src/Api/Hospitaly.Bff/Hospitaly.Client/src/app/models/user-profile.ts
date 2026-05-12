export interface UserProfile {
  userId: string;
  email: string;
  firstName: string;
  lastName: string;
  identityId: string;
  sex: string;
  dateOfBirth: string;
  bloodType: string | null;
  createdOnUtc: string | null;
}
