import {
  Component,
  ChangeDetectionStrategy,
  ElementRef,
  inject,
  AfterViewInit,
} from '@angular/core';
import { RouterLink } from '@angular/router';
import { animate } from 'animejs';

@Component({
  selector: 'app-hero',
  imports: [RouterLink],
  templateUrl: './hero.html',
  styleUrls: ['./hero.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Hero implements AfterViewInit {
  private elementRef = inject(ElementRef);

  ngAfterViewInit(): void {
    this.animateEntrance();
  }

  private animateEntrance(): void {
    const el = this.elementRef.nativeElement;

    animate(el.querySelector('.hero-badge'), {
      translateY: [24, 0],
      opacity: [0, 1],
      duration: 600,
      ease: 'easeOutQuad',
    });

    animate(el.querySelector('.hero-title'), {
      translateY: [60, 0],
      opacity: [0, 1],
      duration: 800,
      delay: 200,
      ease: 'easeOutQuad',
    });

    animate(el.querySelector('.hero-subtitle'), {
      translateY: [30, 0],
      opacity: [0, 1],
      duration: 600,
      delay: 500,
      ease: 'easeOutQuad',
    });

    animate(el.querySelectorAll('.hero-cta .btn'), {
      translateY: [20, 0],
      opacity: [0, 1],
      delay: 700,
      duration: 500,
      ease: 'easeOutQuad',
    });

    animate(el.querySelectorAll('.stat-callout'), {
      translateY: [20, 0],
      opacity: [0, 1],
      delay: 900,
      duration: 500,
      ease: 'easeOutQuad',
    });
  }
}
