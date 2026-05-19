import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { catchError, of, switchMap } from 'rxjs';
import { ClinicOwnershipsService } from '../../services/clinic-ownerships.service';
import { NotificationService } from '../../services/notification.service';
import { ClinicOwnershipResponse, UserSearchResult } from '../../models/clinic-ownership';
import { OwnerCardComponent } from './owner-card/owner-card.component';
import { SearchUserModalComponent } from './transfer/search-user-modal.component';
import { TransferConfirmModalComponent } from './transfer/transfer-confirm-modal.component';

@Component({
  selector: 'app-clinic-ownership-page',
  imports: [RouterLink, OwnerCardComponent, SearchUserModalComponent, TransferConfirmModalComponent],
  templateUrl: './clinic-ownership-page.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ClinicOwnershipPage {
  private readonly route = inject(ActivatedRoute);
  private readonly service = inject(ClinicOwnershipsService);
  private readonly notification = inject(NotificationService);

  readonly ownerships = signal<ClinicOwnershipResponse[]>([]);
  readonly isLoading = signal(true);
  readonly error = signal<string | null>(null);

  readonly sourceForTransfer = signal<ClinicOwnershipResponse | null>(null);
  readonly targetUser = signal<UserSearchResult | null>(null);
  readonly isTransferring = signal(false);

  private clinicId: string | null = null;

  constructor() {
    this.clinicId = this.route.snapshot.paramMap.get('clinicId');

    if (!this.clinicId) {
      this.error.set('Clinic ID not found.');
      this.isLoading.set(false);
      return;
    }

    this.loadOwnerships();
  }

  private loadOwnerships(): void {
    if (!this.clinicId) return;
    this.isLoading.set(true);
    this.error.set(null);
    this.service.getOwnerships(this.clinicId).pipe(
      catchError((err) => {
        this.error.set(err.message ?? 'Failed to load ownership data.');
        return of([]);
      }),
    ).subscribe({
      next: (data) => {
        this.ownerships.set(data);
        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false),
    });
  }

  startTransfer(ownership: ClinicOwnershipResponse): void {
    this.sourceForTransfer.set(ownership);
    this.targetUser.set(null);
  }

  onTargetUserSelected(user: UserSearchResult): void {
    this.targetUser.set(user);
  }

  onSearchModalClose(): void {
    this.sourceForTransfer.set(null);
  }

  onConfirmModalClose(): void {
    this.targetUser.set(null);
  }

  confirmTransfer(data: {
    fromOwnershipId: string;
    targetOwnerIdentityId: string;
    ownerShipType: string;
    percentageToTransfer: number;
    effectiveStart: string;
  }): void {
    if (!this.clinicId || this.isTransferring()) return;
    this.isTransferring.set(true);

    this.service.transferToUser(this.clinicId, data).subscribe({
      next: () => {
        this.isTransferring.set(false);
        this.targetUser.set(null);
        this.sourceForTransfer.set(null);
        this.notification.showSuccess('Ownership transferred successfully.');
        this.loadOwnerships();
      },
      error: (err) => {
        this.isTransferring.set(false);
        this.notification.showError(err.message ?? 'Transfer failed.');
      },
    });
  }
}
