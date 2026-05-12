import { Component, ChangeDetectionStrategy } from '@angular/core';
import { Navbar } from './navbar/navbar';
import { Hero } from './hero/hero';
import { TrustedBy } from './trusted-by/trusted-by';
import { Features } from './features/features';
import { Analytics } from './analytics/analytics';
import { Workflow } from './workflow/workflow';
import { Testimonials } from './testimonials/testimonials';
import { Pricing } from './pricing/pricing';
import { Faq } from './faq/faq';
import { Cta } from './cta/cta';
import { Footer } from './footer/footer';

@Component({
  selector: 'app-landing',
  imports: [
    Navbar,
    Hero,
    TrustedBy,
    Features,
    Analytics,
    Workflow,
    Testimonials,
    Pricing,
    Faq,
    Cta,
    Footer,
  ],
  templateUrl: './landing.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Landing {}
