import { Component, ChangeDetectionStrategy } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { DashboardSidebar } from '../sidebar/dashboard-sidebar';

@Component({
  selector: 'app-dashboard-shell',
  imports: [RouterOutlet, DashboardSidebar],
  templateUrl: './dashboard-shell.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardShell {}
