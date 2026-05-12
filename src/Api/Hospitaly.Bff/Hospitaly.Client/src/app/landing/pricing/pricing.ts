import { Component, AfterViewInit, ChangeDetectionStrategy, signal, ViewChild, ViewChildren, QueryList, ElementRef } from '@angular/core';
import { animate } from 'animejs';

interface PricingFeature {
  label: string;
  included: boolean;
}

interface PricingTier {
  name: string;
  monthlyPrice: number | null;
  annualPrice: number | null;
  custom: boolean;
  highlighted: boolean;
  description: string;
  features: PricingFeature[];
  cta: string;
  badge?: string;
}

@Component({
  selector: 'app-pricing',
  standalone: true,
  templateUrl: './pricing.html',
  styleUrls: ['./pricing.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Pricing implements AfterViewInit {
  readonly isAnnual = signal(false);

  @ViewChild('toggleKnob', { static: true }) toggleKnob!: ElementRef<HTMLElement>;
  @ViewChildren('pricingCard') pricingCards!: QueryList<ElementRef<HTMLElement>>;

  readonly tiers: PricingTier[] = [
    {
      name: 'Starter',
      monthlyPrice: 29,
      annualPrice: 290,
      custom: false,
      highlighted: false,
      description: 'Perfect for small clinics getting started.',
      features: [
        { label: 'Up to 5 doctors', included: true },
        { label: '1,000 patient records', included: true },
        { label: 'Basic analytics', included: true },
        { label: 'Email support', included: true },
        { label: 'SMS reminders', included: false },
        { label: 'Multi-branch support', included: false },
      ],
      cta: 'Get Started',
    },
    {
      name: 'Professional',
      monthlyPrice: 79,
      annualPrice: 790,
      custom: false,
      highlighted: true,
      badge: 'Most Popular',
      description: 'Built for growing practices that need more.',
      features: [
        { label: 'Up to 20 doctors', included: true },
        { label: 'Unlimited patient records', included: true },
        { label: 'Advanced analytics', included: true },
        { label: 'Priority support', included: true },
        { label: 'SMS & Email reminders', included: true },
        { label: 'Multi-branch (up to 3)', included: true },
      ],
      cta: 'Start Free Trial',
    },
    {
      name: 'Enterprise',
      monthlyPrice: null,
      annualPrice: null,
      custom: true,
      highlighted: false,
      description: 'Tailored for large healthcare networks.',
      features: [
        { label: 'Unlimited doctors', included: true },
        { label: 'Unlimited patient records', included: true },
        { label: 'Custom analytics & reports', included: true },
        { label: 'Dedicated account manager', included: true },
        { label: 'Custom integrations', included: true },
        { label: 'SLA & on-premise option', included: true },
      ],
      cta: 'Contact Sales',
    },
  ];

  ngAfterViewInit(): void {
    this.animateGlow();
  }

  toggleBilling(): void {
    this.isAnnual.update(v => !v);
    requestAnimationFrame(() => {
      const knob = this.toggleKnob?.nativeElement;
      if (knob) {
        animate(knob, {
          translateX: this.isAnnual() ? 26 : 0,
          duration: 300,
          easing: 'easeOutCubic',
        });
      }
    });
  }

  private animateGlow(): void {
    const idx = this.tiers.findIndex(t => t.highlighted);
    const card = this.pricingCards.toArray()[idx]?.nativeElement;
    if (!card) return;

    animate(card, {
      keyframes: [
        { boxShadow: '0 0 24px rgba(37, 99, 235, 0.35), 0 0 48px rgba(6, 182, 212, 0.12)' },
        { boxShadow: '0 0 40px rgba(37, 99, 235, 0.55), 0 0 70px rgba(6, 182, 212, 0.25)' },
        { boxShadow: '0 0 24px rgba(37, 99, 235, 0.35), 0 0 48px rgba(6, 182, 212, 0.12)' },
      ],
      duration: 3000,
      loop: true,
      easing: 'easeInOutSine',
    });
  }
}
