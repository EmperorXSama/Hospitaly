# Global Notification / Toast Card System

## Goal
Display a notification card when API responses contain `isSuccess === false`.

## Files to Create

| # | File | Purpose |
|---|---|---|
| 1 | `src/app/models/notification.ts` | `NotificationType` enum + `Notification` interface |
| 2 | `src/app/services/notification.service.ts` | Signal-based service — `show()`, `remove()`, auto-dismiss 10s |
| 3 | `src/app/notification/notification-container.ts` + `.html` | Fixed top-right stacked container with Anime.js animations |
| 4 | `src/app/interceptors/notification.interceptor.ts` | HTTP interceptor — taps responses where `body.isSuccess === false` and fires `NotificationService.showError()` |

## Files to Modify

| # | File | Change |
|---|---|---|
| 1 | `src/app/app.ts` | Add `<app-notification-container />` to template |
| 2 | `src/app/app.config.ts` | Register `notificationInterceptor` in `withInterceptors([...])` |

## Design Tokens Used (from DESIGN.md / styles.css)

- `bg-surface-card` (#1a1a1a) — card background
- `border border-hairline` (#2a2a2a) — card border
- `rounded-xl` (12px) — card radius
- `text-accent-rose` (#ef4444) — error icon + accent strip
- `text-body` (#cccccc) — message text
- `text-muted` (#888888) / hover `text-on-dark` — close button
- `text-title-sm`-like size: `font-semibold text-sm` — title

## Anime.js Animations

- **Entry**: `translateX: ['100%', '0%']`, `opacity: [0, 1]`, `duration: 300`, `ease: 'easeOutCubic'`
- **Exit**: `translateX: ['0%', '100%']`, `opacity: [1, 0]`, `duration: 250`, `ease: 'easeInCubic'`

## Stacking

Multiple notifications in a `flex-col gap-2` container at `fixed top-4 right-4 z-[9999]`.
Each gets a unique ID and independent auto-dismiss timer.

## Notification Service API

```ts
showError(message: string, code?: string): void
showSuccess(message: string): void
show(type: NotificationType, message: string, code?: string): void
remove(id: string): void
notifications = signal<Notification[]>([])
```

## Interceptor

Taps `HttpResponse` events. If `body?.isSuccess === false`, extracts `body.error?.message` (fallback: `"Something went wrong."`) and calls `NotificationService.showError()`.

## Automatic Coverage

Because the interceptor checks all HTTP responses, any API call returning `ApiResponse` with `isSuccess: false` will trigger a notification automatically — no manual wiring needed in components or services.
