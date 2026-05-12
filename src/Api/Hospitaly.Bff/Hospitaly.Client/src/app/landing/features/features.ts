import { Component, AfterViewInit, OnDestroy, ChangeDetectionStrategy, ElementRef, ViewChildren, QueryList } from '@angular/core';
import { animate, stagger } from 'animejs';

interface FeatureItem {
  icon: string;
  title: string;
  description: string;
}

@Component({
  selector: 'app-features',
  templateUrl: './features.html',
  styleUrl: './features.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Features implements AfterViewInit, OnDestroy {
  @ViewChildren('featureCard', { read: ElementRef }) cards!: QueryList<ElementRef<HTMLElement>>;

  features: FeatureItem[] = [
    { icon: '📅', title: 'Appointment Scheduling', description: 'Smart scheduling with automated reminders' },
    { icon: '📋', title: 'Patient Records', description: 'Secure digital records, accessible anytime' },
    { icon: '💰', title: 'Billing & Invoicing', description: 'Automated billing with insurance processing' },
    { icon: '👨‍⚕️', title: 'Doctor Management', description: 'Efficiently manage schedules and specialties' },
    { icon: '📦', title: 'Inventory Tracking', description: 'Real-time inventory with auto-reordering' },
    { icon: '📊', title: 'Reports & Analytics', description: 'Deep insights into clinic performance' },
    { icon: '📧', title: 'SMS/Email Notifications', description: 'Automated patient communication' },
    { icon: '🏥', title: 'Multi-Branch Support', description: 'Manage all branches from one dashboard' },
  ];

  private observer: IntersectionObserver | undefined;

  ngAfterViewInit(): void {
    this.initObserver();
  }

  ngOnDestroy(): void {
    this.observer?.disconnect();
  }

  private initObserver(): void {
    const cardArray = this.cards.toArray().map(c => c.nativeElement);
    if (cardArray.length === 0) return;

    this.observer = new IntersectionObserver(
      (entries) => {
        const visible = entries.filter(e => e.isIntersecting);
        if (visible.length === 0) return;

        const targets = visible.map(e => e.target);
        animate(targets, {
          translateY: [60, 0],
          opacity: [0, 1],
          duration: 800,
          delay: stagger(120),
          ease: 'outCubic',
        });

        visible.forEach(e => this.observer?.unobserve(e.target));
      },
      { threshold: 0.15 },
    );

    cardArray.forEach(el => this.observer?.observe(el));
  }
}
