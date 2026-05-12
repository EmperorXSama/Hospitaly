import { Component, ViewChild, ElementRef, AfterViewInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { animate } from 'animejs';

@Component({
  selector: 'app-cta',
  standalone: true,
  templateUrl: './cta.html',
  styleUrl: './cta.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Cta implements AfterViewInit, OnDestroy {
  @ViewChild('section', { read: ElementRef }) sectionRef!: ElementRef<HTMLElement>;
  @ViewChild('primaryBtn', { read: ElementRef }) primaryBtnRef!: ElementRef<HTMLAnchorElement>;

  private observer: IntersectionObserver | null = null;

  ngAfterViewInit(): void {
    const section = this.sectionRef.nativeElement;
    const btn = this.primaryBtnRef.nativeElement;

    section.style.opacity = '0';
    section.style.transform = 'scale(0.95)';

    this.observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting) {
          animate(section, {
            opacity: 1,
            scale: 1,
            duration: 800,
            easing: 'outCubic',
          });
          this.observer?.disconnect();
        }
      },
      { threshold: 0.2 },
    );
    this.observer.observe(section);

    animate(section, {
      keyframes: [
        { backgroundPosition: '0% 50%', duration: 4000 },
        { backgroundPosition: '100% 50%', duration: 4000 },
        { backgroundPosition: '0% 50%', duration: 4000 },
      ],
      duration: 12000,
      loop: true,
      easing: 'linear',
    });

    animate(btn, {
      keyframes: [
        { boxShadow: '0 0 24px rgba(37, 99, 235, 0.35)', duration: 1000 },
        { boxShadow: '0 0 48px rgba(37, 99, 235, 0.6)', duration: 1000 },
        { boxShadow: '0 0 24px rgba(37, 99, 235, 0.35)', duration: 1000 },
      ],
      duration: 3000,
      loop: true,
      easing: 'inOutSine',
    });
  }

  ngOnDestroy(): void {
    this.observer?.disconnect();
  }
}
