import { Component, ElementRef, inject, OnInit, viewChild } from '@angular/core';
import { NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router } from '@angular/router';
import { animate } from 'animejs';
import { filter } from 'rxjs';

type Direction = 'left' | 'right' | 'top' | 'bottom';

@Component({
  selector: 'app-route-transition',
  template: '<div #overlay class="route-transition-overlay"></div>',
  styles: [`
    .route-transition-overlay {
      position: fixed;
      inset: 0;
      width: 100vw;
      height: 100vh;
      z-index: 9999;
      background-color: #000000;
      pointer-events: none;
      transform: translate(var(--tx, 0), var(--ty, 0));
    }
  `],
})
export class RouteTransition implements OnInit {
  private readonly router = inject(Router);
  private readonly elementRef = inject(ElementRef);
  private readonly overlay = viewChild<ElementRef<HTMLDivElement>>('overlay');

  private direction: Direction = 'left';

  ngOnInit(): void {
    this.router.events
      .pipe(filter((e) => e instanceof NavigationStart))
      .subscribe(() => this.onNavigationStart());

    this.router.events
      .pipe(filter((e) => e instanceof NavigationEnd || e instanceof NavigationCancel || e instanceof NavigationError))
      .subscribe(() => this.onNavigationEnd());
  }

  private onNavigationStart(): void {
    this.direction = this.pickRandomDirection();
    const el = this.overlay();
    if (!el) return;

    el.nativeElement.style.display = '';
    const { enterFrom, enterTo } = this.getOffsets(this.direction, false);

    el.nativeElement.style.setProperty('--tx', `${enterFrom.x}px`);
    el.nativeElement.style.setProperty('--ty', `${enterFrom.y}px`);

    requestAnimationFrame(() => {
      animate(el.nativeElement, {
        translateX: [enterFrom.x, enterTo.x],
        translateY: [enterFrom.y, enterTo.y],
        duration: 400,
        ease: 'easeInOutCubic',
      });
    });
  }

  private onNavigationEnd(): void {
    const el = this.overlay();
    if (!el) return;

    const { exitFrom, exitTo } = this.getOffsets(this.direction, true);

    requestAnimationFrame(() => {
      el.nativeElement.style.setProperty('--tx', `${exitFrom.x}px`);
      el.nativeElement.style.setProperty('--ty', `${exitFrom.y}px`);

      animate(el.nativeElement, {
        translateX: [exitFrom.x, exitTo.x],
        translateY: [exitFrom.y, exitTo.y],
        duration: 400,
        ease: 'easeInOutCubic',
        onComplete: () => {
          el.nativeElement.style.display = 'none';
        },
      });
    });
  }

  private getOffsets(dir: Direction, exiting: boolean): { enterFrom: { x: number; y: number }; enterTo: { x: number; y: number }; exitFrom: { x: number; y: number }; exitTo: { x: number; y: number } } {
    const w = window.innerWidth;
    const h = window.innerHeight;

    switch (dir) {
      case 'left':
        return {
          enterFrom: { x: -w, y: 0 }, enterTo: { x: 0, y: 0 },
          exitFrom: { x: 0, y: 0 }, exitTo: { x: w, y: 0 },
        };
      case 'right':
        return {
          enterFrom: { x: w, y: 0 }, enterTo: { x: 0, y: 0 },
          exitFrom: { x: 0, y: 0 }, exitTo: { x: -w, y: 0 },
        };
      case 'top':
        return {
          enterFrom: { x: 0, y: -h }, enterTo: { x: 0, y: 0 },
          exitFrom: { x: 0, y: 0 }, exitTo: { x: 0, y: h },
        };
      case 'bottom':
        return {
          enterFrom: { x: 0, y: h }, enterTo: { x: 0, y: 0 },
          exitFrom: { x: 0, y: 0 }, exitTo: { x: 0, y: -h },
        };
    }
  }

  private pickRandomDirection(): Direction {
    const directions: Direction[] = ['left', 'right', 'top', 'bottom'];
    return directions[Math.floor(Math.random() * directions.length)];
  }
}
