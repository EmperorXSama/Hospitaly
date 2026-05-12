import { Component, AfterViewInit, ChangeDetectionStrategy, signal, OnDestroy, ViewChildren, QueryList, ElementRef } from '@angular/core';
import { animate } from 'animejs';

interface Testimonial {
  initials: string;
  name: string;
  clinic: string;
  rating: number;
  quote: string;
}

@Component({
  selector: 'app-testimonials',
  standalone: true,
  templateUrl: './testimonials.html',
  styleUrls: ['./testimonials.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Testimonials implements AfterViewInit, OnDestroy {
  readonly testimonials: Testimonial[] = [
    {
      initials: 'SC',
      name: 'Dr. Sarah Chen',
      clinic: 'Palm Grove Medical Center',
      rating: 5,
      quote: 'MediManage transformed our clinic operations. The integrated scheduling and billing saved us countless hours. Our staff loves how intuitive everything is.',
    },
    {
      initials: 'JW',
      name: 'Dr. James Wilson',
      clinic: 'Wilson Family Practice',
      rating: 5,
      quote: 'The analytics dashboard gives us incredible insights into our practice performance. We identified bottlenecks we didn\'t even know we had. A game-changer for data-driven decisions.',
    },
    {
      initials: 'MR',
      name: 'Dr. Maria Rodriguez',
      clinic: 'Community Health Partners',
      rating: 5,
      quote: 'Patient scheduling has never been easier. The automated reminders cut our no-show rate by 60%. Our patients appreciate the convenience of online booking.',
    },
    {
      initials: 'RK',
      name: 'Dr. Robert Kim',
      clinic: 'Advanced Orthopedics',
      rating: 5,
      quote: 'Multi-branch support is a game changer for our practice. We manage three locations seamlessly from a single dashboard. The unified patient records are invaluable.',
    },
    {
      initials: 'EP',
      name: 'Dr. Emily Patel',
      clinic: 'Maple Leaf Pediatrics',
      rating: 5,
      quote: 'The billing automation saved us 20 hours per week. Insurance claim processing that used to take days now happens in minutes. It practically pays for itself.',
    },
  ];

  readonly activeSlide = signal(0);
  readonly isPaused = signal(false);

  @ViewChildren('testimonialCard') cardElements!: QueryList<ElementRef<HTMLElement>>;

  private intervalId?: ReturnType<typeof setInterval>;

  ngAfterViewInit(): void {
    this.startAutoRotate();
    this.animateCurrentCard();
  }

  ngOnDestroy(): void {
    this.stopAutoRotate();
  }

  goToSlide(index: number): void {
    const prev = this.activeSlide();
    if (prev === index) return;

    this.activeSlide.set(index);
    this.animateSlideChange(prev, index);
    this.resetAutoRotate();
  }

  prevSlide(): void {
    const next = this.activeSlide() === 0 ? this.testimonials.length - 1 : this.activeSlide() - 1;
    this.goToSlide(next);
  }

  nextSlide(): void {
    const next = (this.activeSlide() + 1) % this.testimonials.length;
    this.goToSlide(next);
  }

  pauseAutoRotate(): void {
    this.isPaused.set(true);
    this.stopAutoRotate();
  }

  resumeAutoRotate(): void {
    this.isPaused.set(false);
    this.startAutoRotate();
  }

  private startAutoRotate(): void {
    if (this.intervalId) return;
    this.intervalId = setInterval(() => this.nextSlide(), 4000);
  }

  private stopAutoRotate(): void {
    if (this.intervalId) {
      clearInterval(this.intervalId);
      this.intervalId = undefined;
    }
  }

  private resetAutoRotate(): void {
    this.stopAutoRotate();
    this.startAutoRotate();
  }

  private animateSlideChange(prevIndex: number, newIndex: number): void {
    const cards = this.cardElements.toArray();
    const prevCard = cards[prevIndex]?.nativeElement;
    const newCard = cards[newIndex]?.nativeElement;
    if (!prevCard || !newCard) return;

    animate(prevCard, {
      opacity: [1, 0],
      scale: [1, 0.92],
      translateX: newIndex > prevIndex ? [0, -30] : [0, 30],
      duration: 350,
      easing: 'easeOutCubic',
    });

    newCard.style.opacity = '0';
    newCard.style.transform = `scale(0.92) translateX(${newIndex > prevIndex ? 30 : -30}px)`;

    requestAnimationFrame(() => {
      animate(newCard, {
        opacity: [0, 1],
        scale: [0.92, 1],
        translateX: [newIndex > prevIndex ? 30 : -30, 0],
        duration: 400,
        delay: 200,
        easing: 'easeOutCubic',
      });
    });
  }

  private animateCurrentCard(): void {
    const cards = this.cardElements.toArray();
    const card = cards[this.activeSlide()]?.nativeElement;
    if (!card) return;

    animate(card, {
      opacity: [0, 1],
      scale: [0.95, 1],
      translateY: [20, 0],
      duration: 600,
      easing: 'easeOutCubic',
    });
  }
}
