import { Component, ChangeDetectionStrategy } from '@angular/core';

interface TaskItem {
  title: string;
  status: 'in-progress' | 'on-hold' | 'done' | 'pending';
  time: string;
}

@Component({
  selector: 'app-dashboard-tasks-list',
  imports: [],
  templateUrl: './dashboard-tasks-list.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardTasksList {
  readonly tasks: TaskItem[] = [
    { title: 'Patient follow-up calls', status: 'pending', time: '30 min' },
    { title: 'Doctor schedule review', status: 'in-progress', time: '15 min' },
    { title: 'Pending lab results', status: 'on-hold', time: '20 min' },
    { title: 'Insurance approvals', status: 'done', time: '45 min' },
  ];
}
