import { HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { tap } from 'rxjs';
import { NotificationService } from '../services/notification.service';

export const notificationInterceptor: HttpInterceptorFn = (req, next) => {
  const notificationService = inject(NotificationService);

  return next(req).pipe(
    tap((event) => {
      if (event instanceof HttpResponse) {
        const body = event.body as Record<string, unknown> | null;
        if (body && body['isSuccess'] === false) {
          const error = body['error'] as { message?: string } | null;
          notificationService.showError(error?.message ?? 'Something went wrong.');
        }
      }
    }),
  );
};
