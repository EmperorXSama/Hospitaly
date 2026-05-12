import { Component, ChangeDetectionStrategy, inject, AfterViewInit, ElementRef, viewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth';
import { animate, stagger } from 'animejs';
import { DashboardActivityItem } from '../activity-item/dashboard-activity-item';

interface Activity {
  icon: string;
  title: string;
  time: string;
  description: string;
}

@Component({
  selector: 'app-dashboard-activity-panel',
  imports: [RouterLink, DashboardActivityItem],
  templateUrl: './dashboard-activity-panel.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardActivityPanel implements AfterViewInit {
  private readonly authService = inject(AuthService);
  profile = this.authService.profile;

  readonly activityFeed = viewChild.required<ElementRef<HTMLElement>>('activityFeed');

  get initials(): string {
    const name = this.profile()?.userName;
    if (!name) return '?';
    const parts = name.split(' ');
    if (parts.length >= 2) return (parts[0][0] + parts[1][0]).toUpperCase();
    return name[0].toUpperCase();
  }

  get role(): string {
    const roles = this.profile()?.roles;
    if (roles && roles.length > 0) return roles[0];
    return 'User';
  }

  readonly activities: Activity[] = [
    {
      icon: 'calendar',
      title: 'New appointment booked',
      time: '2 min ago',
      description: 'Checkup with John Doe at 3:00 PM — Dr. Sarah Chen',
    },
    {
      icon: 'user-plus',
      title: 'Patient checked in',
      time: '15 min ago',
      description: 'Michael Rodriguez arrived for follow-up consultation',
    },
    {
      icon: 'file',
      title: 'Lab result uploaded',
      time: '42 min ago',
      description: 'Blood work results ready for review — Emily Watson',
    },
    {
      icon: 'clock',
      title: 'Doctor schedule changed',
      time: '1 hour ago',
      description: 'Morning shift swapped — Dr. James Wilson ↔ Dr. Lee',
    },
  ];

  ngAfterViewInit(): void {
    const activities = [...this.activityFeed().nativeElement.children];
    animate(activities, {
      translateX: [-12, 0],
      opacity: [0, 1],
      easing: 'easeOutCubic',
      duration: 400,
      delay: stagger(80),
    });
  }
}
