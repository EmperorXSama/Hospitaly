import { Injectable, signal } from '@angular/core';
import { Notification, NotificationType } from '../models/notification';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  readonly notifications = signal<Notification[]>([]);

  show(type: NotificationType, message: string, code?: string): void {
    const id = crypto.randomUUID();
    this.notifications.update((list) => [...list, { id, type, message, code }]);
    setTimeout(() => this.remove(id), 10_000);
  }

  showError(message: string, code?: string): void {
    this.show(NotificationType.Error, message, code);
  }

  showSuccess(message: string): void {
    this.show(NotificationType.Success, message);
  }

  remove(id: string): void {
    this.notifications.update((list) => list.filter((n) => n.id !== id));
  }
}
