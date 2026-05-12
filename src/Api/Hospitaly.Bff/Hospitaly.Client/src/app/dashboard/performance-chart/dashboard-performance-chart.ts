import { Component, ChangeDetectionStrategy, AfterViewInit, ElementRef, viewChild } from '@angular/core';
import { animate, stagger } from 'animejs';

interface DayData {
  day: string;
  height: number;
}

@Component({
  selector: 'app-dashboard-performance-chart',
  imports: [],
  templateUrl: './dashboard-performance-chart.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardPerformanceChart implements AfterViewInit {
  readonly chartBars = viewChild.required<ElementRef<HTMLElement>>('chartBars');

  readonly days: DayData[] = [
    { day: 'Mon', height: 60 },
    { day: 'Tue', height: 75 },
    { day: 'Wed', height: 45 },
    { day: 'Thu', height: 90 },
    { day: 'Fri', height: 70 },
    { day: 'Sat', height: 40 },
    { day: 'Sun', height: 30 },
  ];

  ngAfterViewInit(): void {
    const bars = [...this.chartBars().nativeElement.children];
    animate(bars, {
      scaleY: [0, 1],
      easing: 'easeOutCubic',
      duration: 600,
      delay: stagger(80),
      transformOrigin: 'bottom center',
    });
  }
}
