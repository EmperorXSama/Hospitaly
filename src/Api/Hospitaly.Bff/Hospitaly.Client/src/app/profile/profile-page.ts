import { Component, ChangeDetectionStrategy, inject, AfterViewInit, ElementRef, viewChild } from '@angular/core';
import { AuthService } from '../services/auth';
import { animate } from 'animejs';

@Component({
  selector: 'app-profile-page',
  imports: [],
  templateUrl: './profile-page.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfilePage implements AfterViewInit {
  private readonly authService = inject(AuthService);

  profile = this.authService.profile;
  user = this.authService.user;

  readonly cardContainer = viewChild.required<ElementRef<HTMLElement>>('cardContainer');

  ngAfterViewInit(): void {
    animate('#profile-header', {
      translateY: [24, 0],
      opacity: [0, 1],
      easing: 'easeOutCubic',
      duration: 600,
    });

    const cards = [...this.cardContainer().nativeElement.querySelectorAll(':scope > *')];
    animate(cards, {
      translateY: [32, 0],
      opacity: [0, 1],
      easing: 'easeOutCubic',
      duration: 500,
      delay: 120,
    });
  }
}
