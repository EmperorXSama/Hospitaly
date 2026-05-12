import { Component, ChangeDetectionStrategy, inject, AfterViewInit, ElementRef, viewChild } from '@angular/core';
import { AuthService } from '../services/auth';
import { animate, stagger } from 'animejs';
import { DashboardMetricCard } from './metric-card/dashboard-metric-card';
import { DashboardPerformanceChart } from './performance-chart/dashboard-performance-chart';
import { DashboardTasksList } from './tasks-list/dashboard-tasks-list';
import { DashboardActivityPanel } from './activity-panel/dashboard-activity-panel';

@Component({
  selector: 'app-dashboard-page',
  imports: [
    DashboardMetricCard,
    DashboardPerformanceChart,
    DashboardTasksList,
    DashboardActivityPanel,
  ],
  templateUrl: './dashboard-page.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardPage implements AfterViewInit {
  private readonly authService = inject(AuthService);
  profile = this.authService.profile;

  readonly metricsGrid = viewChild.required<ElementRef<HTMLElement>>('metricsGrid');
  readonly chartSection = viewChild.required<ElementRef<HTMLElement>>('chartSection');

  today = new Date();
  formattedDate = this.today.toLocaleDateString('en-US', {
    weekday: 'long',
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  });

  readonly metricCards = [
    {
      value: '18',
      label: 'Appointments Today',
      delta: '+12%',
      deltaUp: true,
      icon: 'calendar',
    },
    {
      value: '12',
      label: 'Patients Checked In',
      delta: '+8%',
      deltaUp: true,
      icon: 'users',
    },
    {
      value: '6',
      label: 'Pending Consultations',
      delta: '-3%',
      deltaUp: false,
      icon: 'clock',
    },
    {
      value: '93%',
      label: 'Clinic Efficiency',
      delta: '+2%',
      deltaUp: true,
      icon: 'trending',
    },
  ];

  ngAfterViewInit(): void {
    const metricEls = [...this.metricsGrid().nativeElement.children];
    animate(metricEls, {
      translateY: [24, 0],
      opacity: [0, 1],
      easing: 'easeOutCubic',
      duration: 600,
      delay: stagger(100),
    });

    animate(this.chartSection().nativeElement, {
      translateY: [20, 0],
      opacity: [0, 1],
      easing: 'easeOutCubic',
      duration: 500,
      delay: 400,
    });
  }
}
