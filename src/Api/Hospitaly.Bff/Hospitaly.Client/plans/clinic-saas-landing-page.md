# Clinic Management SaaS Landing Page — Implementation Plan

## Overview

Build a modern, premium, animated landing page for a Clinic Management Application using **Angular 21** (standalone components), **TailwindCSS v4**, and **Anime.js**.

---

## 1. Install Dependencies

- `npm install animejs`
- Add **Inter** font via Google Fonts in `index.html`:
  ```html
  <link rel="preconnect" href="https://fonts.googleapis.com" />
  <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&display=swap" rel="stylesheet" />
  ```

---

## 2. Route Changes (`src/app/app.routes.ts`)

- `path: ''` → load `LandingComponent` (new, lazy-loaded)
- `path: 'login'` → existing Login (unchanged)
- `path: 'home'` → existing Home (unchanged, guarded by `authGuard`)
- `path: '**'` → redirect to `''`

---

## 3. File Structure

```
src/app/landing/
├── landing.ts              # Container component — imports all sections
├── landing.html
├── landing.css
├── navbar/
│   ├── navbar.ts
│   ├── navbar.html
│   └── navbar.css
├── hero/
│   ├── hero.ts
│   ├── hero.html
│   └── hero.css
├── trusted-by/
│   ├── trusted-by.ts
│   ├── trusted-by.html
│   └── trusted-by.css
├── features/
│   ├── features.ts
│   ├── features.html
│   └── features.css
├── analytics/
│   ├── analytics.ts
│   ├── analytics.html
│   └── analytics.css
├── workflow/
│   ├── workflow.ts
│   ├── workflow.html
│   └── workflow.css
├── testimonials/
│   ├── testimonials.ts
│   ├── testimonials.html
│   └── testimonials.css
├── pricing/
│   ├── pricing.ts
│   ├── pricing.html
│   └── pricing.css
├── faq/
│   ├── faq.ts
│   ├── faq.html
│   └── faq.css
├── cta/
│   ├── cta.ts
│   ├── cta.html
│   └── cta.css
└── footer/
    ├── footer.ts
    ├── footer.html
    └── footer.css
```

---

## 4. Global Styles (`src/styles.css`)

- Import TailwindCSS v4 (`@import "tailwindcss"`)
- Import Inter font
- Define custom theme in CSS:

```css
/* Concept — exact syntax per Tailwind v4 */
@import "tailwindcss";
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&display=swap');

@theme {
  --font-sans: 'Inter', ui-sans-serif, system-ui, sans-serif;
  --color-primary: #2563eb;
  --color-primary-light: #3b82f6;
  --color-primary-dark: #1d4ed8;
  --color-secondary: #0f172a;
  --color-accent: #06b6d4;
  --color-surface: #1e293b;
  --color-surface-light: #334155;
}
```

- Add global utility layers for:
  - `.glass` — glassmorphism cards (`backdrop-blur-xl`, semi-transparent bg, border)
  - `.blob` — animated gradient blob shapes
  - `.text-gradient` — gradient text for hero headlines
  - `.grid-bg` — subtle dot/grid background pattern

---

## 5. Color Palette

| Token | Hex | Usage |
|-------|-----|-------|
| `primary` | `#2563eb` | CTAs, highlights, active states |
| `primary-light` | `#3b82f6` | Hover states, gradient stops |
| `primary-dark` | `#1d4ed8` | Active/pressed states |
| `secondary` | `#0f172a` | Dark section backgrounds |
| `accent` | `#06b6d4` | Accent elements, badges, secondary highlights |
| `surface` | `#1e293b` | Card backgrounds (dark sections) |
| `surface-light` | `#334155` | Elevated cards, hover states |
| `text-primary` | `#f8fafc` | Headlines on dark |
| `text-secondary` | `#94a3b8` | Body text on dark |
| `text-muted` | `#64748b` | Captions, metadata |

---

## 6. Component Specifications

### 6.1 Navbar (`navbar/`)
- **Elements:** Logo (SVG or text "MediManage"), nav links (Features, Pricing, Testimonials, Contact), Login button, "Start Free Trial" CTA
- **Styling:** Transparent bg → `backdrop-blur-xl` + bg opacity on scroll. Dark glass style.
- **Mobile:** Hamburger toggle → slide-in drawer from right
- **Animations:** Scroll event → Anime.js transition of bg/blur; mobile menu slide with anime

### 6.2 Hero (`hero/`)
- **Elements:** Large headline ("Modern Clinic Management For Smarter Healthcare"), supporting subtitle, 2 CTA buttons, animated dashboard preview
- **Dashboard mockup:** Realistic preview with floating cards — Appointment Analytics, Patient Statistics, Doctor Schedules, Revenue Chart, Live Activity widget
- **Background:** Animated gradient blobs, subtle grid pattern
- **Animations:**
  - Staggered entrance: headline → subtitle → CTAs (opacity + translateY via Anime.js with stagger)
  - Dashboard mockup: fade-in + scale from right
  - Floating cards: gentle Y-axis drift (anime loop, alternate)
  - Mouse parallax: dashboard tilts slightly on mouse move

### 6.3 Trusted By (`trusted-by/`)
- **Elements:** Row of clinic/healthcare brand logos (mock SVGs), trust badges (HIPAA, ISO, SOC2)
- **Animations:** Infinite auto-scroll marquee with anime; pause on hover

### 6.4 Features (`features/`)
- **Elements:** 8 feature cards in 4×2 grid:
  1. Appointment Scheduling
  2. Patient Records
  3. Billing & Invoicing
  4. Doctor Management
  5. Inventory Tracking
  6. Reports & Analytics
  7. SMS/Email Notifications
  8. Multi-Branch Support
- **Styling:** Glass cards with icon, title, description
- **Animations:** Staggered reveal via IntersectionObserver + Anime.js; hover → lift + glow + icon pulse

### 6.5 Analytics Showcase (`analytics/`)
- **Elements:** Dashboard mockup showing:
  - Revenue card (with counter)
  - Appointments trend graph (bar chart animation)
  - Patient growth (line graph)
  - Doctor performance (horizontal bars)
- **Animations:** Counter-up on scroll, chart bars animate width/height from 0, cards stagger in

### 6.6 Workflow (`workflow/`)
- **Elements:** 3 steps: Register Clinic → Manage Patients → Grow Operations
- **Styling:** Numbered circles, connecting line with arrow, description cards alternating sides
- **Animations:** Steps reveal on scroll, connecting line draws with anime, step numbers pulse

### 6.7 Testimonials (`testimonials/`)
- **Elements:** Avatar, name, clinic name, star rating, quote. 4-5 cards.
- **Styling:** Glass cards with avatar circles, horizontal carousel
- **Animations:** Auto-rotate every 4s (anime timeline), slide transition, pause on hover, active card glow

### 6.8 Pricing (`pricing/`)
- **Elements:** 3 tiers:
  - Starter — $29/mo (or $290/yr)
  - Professional — $79/mo (or $790/yr) — **highlighted**
  - Enterprise — Custom pricing
- **Details:** Feature lists per tier, CTA buttons, monthly/annual toggle
- **Animations:** Professional card has persistent glow border (anime pulse), hover lift on all cards, toggle switch animation

### 6.9 FAQ (`faq/`)
- **Elements:** 6-8 common questions with expandable answers
- **Styling:** Clean bordered accordion rows
- **Animations:** Smooth height expand/collapse via Anime.js (animate from 0 to scrollHeight), icon rotation

### 6.10 Final CTA (`cta/`)
- **Elements:** Large headline "Start Managing Your Clinic Smarter Today", subtitle, primary + secondary CTA buttons
- **Background:** Full-width gradient with animated shift
- **Animations:** CTA button glow pulse (anime keyframes), gradient shift, entrance zoom+fade

### 6.11 Footer (`footer/`)
- **Elements:** Multi-column links, social icons, contact info, copyright, product links
- **Styling:** Dark bg with subtle top border, clean text hierarchy
- **Animations:** Minimal — subtle fade on link hover, no heavy motion

---

## 7. Animation Architecture

### Scroll-Triggered System

Use a shared `AnimationService` (or inline `IntersectionObserver` in each component):

```typescript
// Pattern:
const observer = new IntersectionObserver((entries) => {
  entries.forEach(entry => {
    if (entry.isIntersecting) {
      anime({ targets: ..., ...params });
      observer.unobserve(entry.target);
    }
  });
}, { threshold: 0.2 });
```

### Anime.js Usage Patterns

| Animation | Targets | Properties | Easing |
|-----------|---------|------------|--------|
| Entrance fade-up | Elements | `opacity: [0,1], translateY: [40,0]` | `easeOutExpo` |
| Staggered reveal | Children | `opacity: [0,1], translateY: [30,0]`, stagger delay | `easeOutQuad` |
| Floating drift | Cards | `translateY: [-8, 8]`, loop, alternate | `easeInOutSine` |
| Counter up | Numbers | `innerHTML` from 0 to target, round: 1 | `easeOutQuad` |
| Chart bars | Bars | `scaleY: [0, 1]` or `height: [0, target]` | `easeOutCubic` |
| Accordion | Content | `height: [0, scrollHeight]` | `easeOutCubic` |
| Glow pulse | CTA/Plan | `boxShadow` keyframes, loop | `easeInOutSine` |
| Marquee scroll | Logo row | `translateX` loop | `linear` |

---

## 8. Responsive Strategy

| Breakpoint | Layout Adjustments |
|------------|-------------------|
| **1280px+** | Full grid, all effects active, max-width containers |
| **1024px** | Features: 2-col grid. Pricing: tighter 3-col. Dashboard shrinks |
| **768px** | Single column. Hamburger nav. Hero text smaller. Features stack vertically. Pricing stacks in list. Analytics simplified. |
| **480px** | Extra spacing tightening. Smaller font sizes. CTAs full-width |

---

## 9. Implementation Order

1. Install animejs, add Inter font to index.html
2. Create `LandingComponent` container + route registration
3. Build **Navbar** (scroll blur, mobile menu)
4. Build **Hero** (dashboard mockup, floating cards, blobs)
5. Build **TrustedBy** (marquee logos)
6. Build **Features** (glass cards, hover effects)
7. Build **Analytics** (charts, counters)
8. Build **Workflow** (steps, connecting line)
9. Build **Testimonials** (carousel)
10. Build **Pricing** (tiers, glow, toggle)
11. Build **FAQ** (accordion)
12. Build **CTA** (gradient, glow)
13. Build **Footer**
14. Wire scroll-triggered animations + mouse parallax across all sections
15. Responsive polish across all breakpoints
16. Run `ng serve` to verify

---

## 10. Key Angular Patterns

- All components are **standalone**
- Use modern Angular control flow: `@for`, `@if`, `@defer`
- Use **signals** for any reactive state (mobile menu open, active FAQ item, active pricing tier)
- Use `ngAfterViewInit` for Anime.js initialization (targets must be in DOM)
- Use `@ViewChildren` / `@ViewChild` + `ElementRef` for Anime.js element references
- Lazy-load the landing page module via route `loadComponent`
