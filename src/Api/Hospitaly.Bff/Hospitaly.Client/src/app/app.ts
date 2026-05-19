import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { RouteTransition } from './route-transition/route-transition';
import { NotificationContainer } from './notification/notification-container';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouteTransition, NotificationContainer],
  template: `<router-outlet /><app-route-transition /><app-notification-container />`,
})
export class App {}
