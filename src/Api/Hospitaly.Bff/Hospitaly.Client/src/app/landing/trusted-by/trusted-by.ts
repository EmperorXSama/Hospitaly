import { Component, AfterViewInit, OnDestroy, HostListener, ElementRef, ChangeDetectionStrategy } from '@angular/core';
import { animate, JSAnimation } from 'animejs';

interface TrustBadge {
  label: string;
  icon: string;
}

@Component({
  selector: 'app-trusted-by',
  templateUrl: './trusted-by.html',
  styleUrl: './trusted-by.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TrustedBy implements AfterViewInit, OnDestroy {
  clinics = [
    'City Medical Center',
    'Harmony Health',
    'Pulse Clinic',
    'Apex Dental',
    'VitalCare',
    'NovaMed',
    'BlueCross Health',
    'Summit Pediatrics',
    'Elite Orthopedics',
    'PrimeCare',
  ];

  badges: TrustBadge[] = [
    { label: 'HIPAA Compliant', icon: '🛡️' },
    { label: 'ISO 27001', icon: '✓' },
    { label: 'SOC 2 Type II', icon: '🔒' },
  ];

  private marqueeAnim: JSAnimation | undefined;

  constructor(private el: ElementRef) {}

  ngAfterViewInit(): void {
    this.initMarquee();
  }

  ngOnDestroy(): void {
    this.marqueeAnim?.pause();
  }

  @HostListener('mouseenter')
  onMouseEnter(): void {
    this.marqueeAnim?.pause();
  }

  @HostListener('mouseleave')
  onMouseLeave(): void {
    this.marqueeAnim?.play();
  }

  private initMarquee(): void {
    const track = this.el.nativeElement.querySelector('.marquee-track') as HTMLElement;
    if (!track) return;

    this.marqueeAnim = animate(track, {
      translateX: ['0%', '-50%'],
      duration: 30000,
      loop: true,
      ease: 'linear',
    });
  }
}
