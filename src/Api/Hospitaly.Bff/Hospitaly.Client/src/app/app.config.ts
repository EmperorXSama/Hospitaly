import { APP_INITIALIZER, ApplicationConfig, inject } from '@angular/core';
import { provideRouter, Router } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { routes } from './app.routes';
import { credentialsInterceptor } from './interceptors/credentials.interceptor';
import { notificationInterceptor } from './interceptors/notification.interceptor';
import { AuthService } from './services/auth';

function initializeAuth(): () => Promise<unknown> {
  const authService = inject(AuthService);
  const router = inject(Router);
  return () => {
    return firstValueFrom(authService.checkSession()).then(() => {
      const profile = authService.profile();
      const url = window.location.pathname;
      if (profile?.requiresOnboarding && url !== '/onboarding') {
        router.navigateByUrl('/onboarding', { replaceUrl: true });
      }
    });
  };
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([credentialsInterceptor, notificationInterceptor])),
    {
      provide: APP_INITIALIZER,
      useFactory: initializeAuth,
      multi: true,
    },
  ],
};
