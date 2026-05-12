import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { RouteTransition } from './route-transition/route-transition';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouteTransition],
  template: `<router-outlet /><app-route-transition />`,
})
export class App {}
