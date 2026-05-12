export interface ClientUserData {
  userId: string;
  userName: string;
  email: string;
  roles: readonly string[];
  permissions: readonly string[];
  requiresOnboarding: boolean;
}
