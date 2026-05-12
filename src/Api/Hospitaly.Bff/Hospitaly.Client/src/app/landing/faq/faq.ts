import { Component, signal, ViewChildren, ElementRef, QueryList, AfterViewInit, ChangeDetectionStrategy } from '@angular/core';
import { animate } from 'animejs';

interface FaqItem {
  question: string;
  answer: string;
}

@Component({
  selector: 'app-faq',
  standalone: true,
  templateUrl: './faq.html',
  styleUrl: './faq.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Faq implements AfterViewInit {
  activeIndex = signal<number | null>(null);

  @ViewChildren('content', { read: ElementRef }) contents!: QueryList<ElementRef<HTMLDivElement>>;

  items: FaqItem[] = [
    {
      question: 'How do I get started?',
      answer: 'Sign up for a free trial. No credit card required. You can start scheduling appointments, managing patient records, and billing within minutes. Our guided onboarding walks you through every step.',
    },
    {
      question: 'Is my data secure?',
      answer: 'Yes, we use bank-level encryption (AES-256) for data at rest and TLS 1.3 for data in transit. We are HIPAA-compliant and undergo regular third-party security audits to ensure your patients\u2019 information stays protected.',
    },
    {
      question: 'Can I import existing patient records?',
      answer: 'Absolutely. We support CSV, Excel, and HL7 imports. Our migration team can help transfer your existing data from any major EHR system, usually within 48 hours. Just contact our support team to get started.',
    },
    {
      question: 'Do you offer training?',
      answer: 'Yes, we provide onboarding sessions for all new customers. Our Starter plan includes self-paced video tutorials, while Professional and Enterprise plans include live virtual training sessions for your entire staff.',
    },
    {
      question: 'Can I customize the system?',
      answer: 'Our Professional and Enterprise plans offer extensive customization options including custom fields, templates, workflows, reporting dashboards, and branded patient portals. The Starter plan includes essential configuration options.',
    },
    {
      question: 'What kind of support do you offer?',
      answer: 'Email support on Starter, priority support on Professional with 4-hour response time, and dedicated account management on Enterprise with 1-hour response time and 24/7 phone support.',
    },
    {
      question: 'Is there a mobile app?',
      answer: 'Yes, we have iOS and Android apps available for free on the App Store and Google Play. Your staff can access schedules, patient info, and communications on the go, with full offline support.',
    },
  ];

  toggle(index: number): void {
    const contents = this.contents.toArray();
    const prev = this.activeIndex();

    if (prev === index) {
      this.activeIndex.set(null);
      if (contents[index]) {
        animate(contents[index].nativeElement, {
          height: 0,
          duration: 300,
          easing: 'outCubic',
        });
      }
    } else {
      this.activeIndex.set(index);
      if (prev !== null && contents[prev]) {
        animate(contents[prev].nativeElement, {
          height: 0,
          duration: 300,
          easing: 'outCubic',
        });
      }
      if (contents[index]) {
        const el = contents[index].nativeElement;
        el.style.height = 'auto';
        const targetHeight = el.scrollHeight;
        el.style.height = '0px';
        requestAnimationFrame(() => {
          animate(el, {
            height: targetHeight,
            duration: 300,
            easing: 'outCubic',
          });
        });
      }
    }
  }

  ngAfterViewInit(): void {
    this.contents.forEach(({ nativeElement: el }) => {
      el.style.height = '0px';
    });
  }
}
