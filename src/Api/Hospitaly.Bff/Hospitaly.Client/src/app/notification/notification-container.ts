import { Component, ChangeDetectionStrategy, inject, viewChildren, ElementRef, afterEveryRender } from '@angular/core';
import { animate } from 'animejs';
import { NotificationService } from '../services/notification.service';
import { NotificationType } from '../models/notification';

@Component({
  selector: 'app-notification-container',
  imports: [],
  templateUrl: './notification-container.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NotificationContainer {
  private readonly notificationService = inject(NotificationService);
  readonly notifications = this.notificationService.notifications;
  readonly NotificationType = NotificationType;

  private readonly cardElements = viewChildren<ElementRef<HTMLDivElement>>('card');
  private readonly animatedIds = new Set<string>();

  constructor() {
    afterEveryRender(() => {
      for (const card of this.cardElements()) {
        const id = card.nativeElement.dataset['notificationId'];
        if (id && !this.animatedIds.has(id)) {
          this.animatedIds.add(id);
          animate(card.nativeElement, {
            translateX: ['100%', '0%'],
            opacity: [0, 1],
            duration: 300,
            ease: 'easeOutCubic',
          });
        }
      }
    });
  }

  dismiss(id: string): void {
    const card = this.cardElements().find(
      (el) => el.nativeElement.dataset['notificationId'] === id,
    );
    if (!card) return;

    animate(card.nativeElement, {
      translateX: ['0%', '100%'],
      opacity: [1, 0],
      duration: 250,
      ease: 'easeInCubic',
      complete: () => this.notificationService.remove(id),
    });
  }
}
