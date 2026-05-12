import { Component, AfterViewInit, ChangeDetectionStrategy, ViewChildren, ViewChild, QueryList, ElementRef, OnDestroy } from '@angular/core';
import { animate } from 'animejs';

interface Step {
  number: number;
  title: string;
  description: string;
}

@Component({
  selector: 'app-workflow',
  standalone: true,
  templateUrl: './workflow.html',
  styleUrls: ['./workflow.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Workflow implements AfterViewInit, OnDestroy {
  readonly steps: Step[] = [
    { number: 1, title: 'Register Your Clinic', description: 'Set up your clinic profile in minutes' },
    { number: 2, title: 'Manage Patients', description: 'Streamline patient management with digital tools' },
    { number: 3, title: 'Grow Operations', description: 'Scale your practice with powerful analytics' },
  ];

  @ViewChildren('stepCircle') circles!: QueryList<ElementRef<HTMLElement>>;
  @ViewChildren('stepCard') cards!: QueryList<ElementRef<HTMLElement>>;
  @ViewChild('connectLine', { static: true }) line!: ElementRef<HTMLElement>;

  private observer?: IntersectionObserver;
  private hasAnimated = false;

  ngAfterViewInit(): void {
    const host = this.line?.nativeElement?.closest('section');
    if (!host) return;

    this.observer = new IntersectionObserver(
      (entries) => {
        if (entries.some(e => e.isIntersecting) && !this.hasAnimated) {
          this.hasAnimated = true;
          this.animateLine();
          this.animateCircles();
          this.cards.forEach(c => c.nativeElement.classList.add('step-visible'));
        }
      },
      { threshold: 0.2 }
    );
    this.observer.observe(host);
  }

  ngOnDestroy(): void {
    this.observer?.disconnect();
  }

  private animateLine(): void {
    const el = this.line!.nativeElement;
    const isMobile = window.innerWidth < 768;
    animate(el, {
      [isMobile ? 'scaleY' : 'scaleX']: [0, 1],
      duration: 1200,
      easing: 'easeInOutCubic',
    });
  }

  private animateCircles(): void {
    this.circles.forEach((c, i) => {
      const el = c.nativeElement;
      el.style.opacity = '0';
      requestAnimationFrame(() => {
        el.style.opacity = '1';
        animate(el, {
          scale: [0.5, 1.15, 1],
          opacity: [0, 1],
          duration: 800,
          delay: i * 200 + 300,
          easing: 'easeOutCubic',
        });
      });
    });
  }
}
