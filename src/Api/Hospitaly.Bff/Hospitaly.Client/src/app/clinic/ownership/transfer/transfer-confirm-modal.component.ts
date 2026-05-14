import { ChangeDetectionStrategy, Component, computed, input, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ClinicOwnershipResponse, UserSearchResult } from '../../../models/clinic-ownership';

@Component({
  selector: 'app-transfer-confirm-modal',
  imports: [FormsModule],
  templateUrl: './transfer-confirm-modal.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TransferConfirmModalComponent {
  readonly sourceOwnership = input.required<ClinicOwnershipResponse>();
  readonly targetUser = input.required<UserSearchResult>();
  readonly close = output<void>();
  readonly confirm = output<{
    fromOwnershipId: string;
    targetOwnerIdentityId: string;
    ownerShipType: string;
    percentageToTransfer: number;
    effectiveStart: string;
  }>();

  readonly percentage = signal<number>(0);
  readonly ownerShipType = signal<string>('CoOwner');
  readonly effectiveStart = signal<string>(new Date().toISOString().split('T')[0]);
  readonly isSubmitting = signal(false);

  readonly maxPercentage = computed(() => this.sourceOwnership().sharePercentage - 1);
  readonly retainedPercentage = () => this.sourceOwnership().sharePercentage - this.percentage();

  get canSubmit(): boolean {
    const pct = this.percentage();
    const max = this.sourceOwnership().sharePercentage - 1;
    return pct > 0 && pct <= max && this.effectiveStart().length > 0;
  }

  submit(): void {
    if (!this.canSubmit || this.isSubmitting()) return;
    this.isSubmitting.set(true);
    this.confirm.emit({
      fromOwnershipId: this.sourceOwnership().id,
      targetOwnerIdentityId: this.targetUser().identityId,
      ownerShipType: this.ownerShipType(),
      percentageToTransfer: this.percentage(),
      effectiveStart: new Date(this.effectiveStart()).toISOString(),
    });
  }
}
