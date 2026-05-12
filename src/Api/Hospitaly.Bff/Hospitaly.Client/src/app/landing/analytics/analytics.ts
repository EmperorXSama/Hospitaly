import { Component, AfterViewInit, ChangeDetectionStrategy, ElementRef, ViewChildren, QueryList, signal } from '@angular/core';
import { animate, stagger } from 'animejs';

interface CounterData {
  label: string;
  target: number;
  prefix: string;
  suffix: string;
  icon: string;
}

interface BarData {
  label: string;
  value: number;
}

interface DoctorData {
  name: string;
  specialty: string;
  patients: number;
  rating: number;
}

@Component({
  selector: 'app-analytics',
  templateUrl: './analytics.html',
  styleUrl: './analytics.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Analytics implements AfterViewInit {
  @ViewChildren('counterEl', { read: ElementRef }) counterEls!: QueryList<ElementRef<HTMLElement>>;
  @ViewChildren('barEl', { read: ElementRef }) barEls!: QueryList<ElementRef<HTMLElement>>;
  @ViewChildren('doctorCard', { read: ElementRef }) doctorCards!: QueryList<ElementRef<HTMLElement>>;

  hasAnimated = signal(false);

  counters: CounterData[] = [
    { label: 'Revenue', target: 2840000, prefix: '$', suffix: '+', icon: '💰' },
    { label: 'Patients', target: 12450, prefix: '', suffix: '+', icon: '👥' },
    { label: 'Appointments', target: 48720, prefix: '', suffix: '+', icon: '📅' },
    { label: 'Doctors', target: 156, prefix: '', suffix: '', icon: '👨‍⚕️' },
  ];

  weeklyAppointments: BarData[] = [
    { label: 'Mon', value: 85 },
    { label: 'Tue', value: 92 },
    { label: 'Wed', value: 78 },
    { label: 'Thu', value: 95 },
    { label: 'Fri', value: 88 },
    { label: 'Sat', value: 45 },
    { label: 'Sun', value: 20 },
  ];

  patientGrowth: BarData[] = [
    { label: 'Jan', value: 320 },
    { label: 'Feb', value: 450 },
    { label: 'Mar', value: 580 },
    { label: 'Apr', value: 720 },
    { label: 'May', value: 890 },
    { label: 'Jun', value: 1100 },
  ];

  doctors: DoctorData[] = [
    { name: 'Dr. Sarah Chen', specialty: 'Cardiology', patients: 142, rating: 4.9 },
    { name: 'Dr. James Wilson', specialty: 'Pediatrics', patients: 128, rating: 4.8 },
    { name: 'Dr. Maria Garcia', specialty: 'Orthopedics', patients: 115, rating: 4.7 },
    { name: 'Dr. Robert Kim', specialty: 'Neurology', patients: 98, rating: 4.9 },
  ];

  private observer: IntersectionObserver | undefined;

  ngAfterViewInit(): void {
    this.initObserver();
  }

  private initObserver(): void {
    const section = document.getElementById('analytics-section');
    if (!section) return;

    this.observer = new IntersectionObserver(
      (entries) => {
        const entry = entries[0];
        if (entry.isIntersecting && !this.hasAnimated()) {
          this.hasAnimated.set(true);
          this.animateCounters();
          this.animateBars();
          this.animateDoctorCards();
          this.observer?.disconnect();
        }
      },
      { threshold: 0.2 },
    );

    this.observer.observe(section);
  }

  private animateCounters(): void {
    const elements = this.counterEls.toArray();
    elements.forEach((el, i) => {
      const counter = this.counters[i];
      const obj = { value: 0 };

      animate(obj, {
        value: counter.target,
        duration: 2000,
        delay: i * 200,
        ease: 'outExpo',
        modifier: (v: number) => Math.round(v),
        onUpdate: () => {
          el.nativeElement.innerText = this.formatDisplayValue(Math.round(obj.value), counter);
        },
      });
    });
  }

  private animateBars(): void {
    const barElements = this.barEls.toArray();
    barElements.forEach((bar, i) => {
      const barInner = bar.nativeElement.querySelector('.bar-fill') as HTMLElement;
      if (!barInner) return;

      animate(barInner, {
        scaleY: [0, 1],
        duration: 800,
        delay: i * 60,
        ease: 'outCubic',
      });
    });
  }

  private animateDoctorCards(): void {
    const cardElements = this.doctorCards.toArray().map(c => c.nativeElement);
    animate(cardElements, {
      translateY: [40, 0],
      opacity: [0, 1],
      duration: 600,
      delay: stagger(100),
      ease: 'outCubic',
    });
  }

  private formatDisplayValue(value: number, counter: CounterData): string {
    const formatted = value.toLocaleString();
    return `${counter.prefix}${formatted}${counter.suffix}`;
  }

  getBarHeightPercent(value: number, max: number): number {
    if (max === 0) return 0;
    return (value / max) * 100;
  }

  getInitials(name: string): string {
    const parts = name.split(' ');
    if (parts.length < 2) return name.charAt(0);
    return parts[0].charAt(1) + parts[1].charAt(0);
  }
}
