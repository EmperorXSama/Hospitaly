export interface OwnerInfo {
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
  sex: string;
  dateOfBirth: string;
}

export interface ClinicOwnershipResponse {
  id: string;
  ownerId: string;
  ownerShipType: string;
  sharePercentage: number;
  effectiveStart: string;
  effectiveEnd: string | null;
  status: string;
  owner: OwnerInfo | null;
}

export interface UserSearchResult {
  userId: string;
  email: string;
  firstName: string;
  lastName: string;
  identityId: string;
  sex: string;
  dateOfBirth: string;
}

export interface TransferOwnershipRequest {
  fromOwnershipId: string;
  targetOwnerIdentityId: string;
  ownerShipType: string;
  percentageToTransfer: number;
  effectiveStart: string;
}
