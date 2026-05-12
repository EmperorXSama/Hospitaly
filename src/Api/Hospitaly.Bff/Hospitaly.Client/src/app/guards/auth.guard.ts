import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth';
import { map, of } from 'rxjs';

export const authGuard: CanActivateFn = () => {
  const auth = inject(AuthService);

  if (auth.isAuthenticated()) {
    return of(true);
  }

  return auth.checkSession().pipe(
    map((claims) => {
      if (claims) return true;
      auth.login();
      return false;
    }),
  );
};
