import { ChangeDetectionStrategy, Component, inject, input, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ClinicOwnershipResponse, UserSearchResult } from '../../../models/clinic-ownership';
import { ClinicOwnershipsService } from '../../../services/clinic-ownerships.service';

@Component({
  selector: 'app-search-user-modal',
  imports: [FormsModule],
  templateUrl: './search-user-modal.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SearchUserModalComponent {
  private readonly service = inject(ClinicOwnershipsService);

  readonly sourceOwnership = input.required<ClinicOwnershipResponse>();
  readonly close = output<void>();
  readonly userSelected = output<UserSearchResult>();

  readonly searchQuery = signal('');
  readonly results = signal<UserSearchResult[]>([]);
  readonly isSearching = signal(false);
  readonly hasSearched = signal(false);

  search(): void {
    const q = this.searchQuery().trim();
    if (q.length < 2) return;

    this.isSearching.set(true);
    this.hasSearched.set(false);
    this.service.searchUsers(q).subscribe({
      next: (users) => {
        this.results.set(users);
        this.isSearching.set(false);
        this.hasSearched.set(true);
      },
      error: () => {
        this.isSearching.set(false);
        this.hasSearched.set(true);
      },
    });
  }

  select(user: UserSearchResult): void {
    this.userSelected.emit(user);
  }
}
