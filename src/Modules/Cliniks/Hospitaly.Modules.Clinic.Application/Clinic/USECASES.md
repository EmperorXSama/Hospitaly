# Clinic Aggregate — Use Cases

## Commands (Write Side)

### 1. Create Clinic
- **Reason**: Onboard a new clinic into the system
- **Input**: `ClinicInfo` (Name, TradingName, Description, LogoUrl?), `ClinicAddress` (Street, City, Region?, PostalCode?, Country, Latitude?, Longitude?), `ClinicContactInfo` (PhoneNumber?, Email?, Website?), `OperatingHours[]`
- **Output**: `ClinicId` (Guid)

### 2. Update Clinic Info
- **Reason**: Clinic name/description changes
- **Input**: `ClinicId`, `ClinicInfo`
- **Output**: `Success`

### 3. Update Clinic Address
- **Reason**: Clinic relocates
- **Input**: `ClinicId`, `ClinicAddress`
- **Output**: `Success`

### 4. Update Clinic Contact Info
- **Reason**: Phone/email/website changes
- **Input**: `ClinicId`, `ClinicContactInfo`
- **Output**: `Success`

### 5. Set Operating Hours
- **Reason**: Clinic changes its weekly schedule
- **Input**: `ClinicId`, `OperatingHours[]` (per DayOfWeek, with optional OperatingTimeRange + IsResting flag)
- **Output**: `Success`

### 6. Change Clinic Status
- **Reason**: Suspend, deactivate, or permanently close the clinic
- **Input**: `ClinicId`, `ClinicStatus` (Active|Inactive|Suspended|PermanentlyClosed)
- **Output**: `Success`

### 7. Add License
- **Reason**: Register a new operating license
- **Input**: `ClinicId`, `LicenseNumber`, `IssuingAuthority`, `LicenseType`, `LicenceValidityPeriod` (Start, End), `LicenceAdministrativeStatus`
- **Output**: `LicenseId` (Guid)

### 8. Update License Administrative Status
- **Reason**: Regulatory body suspends/revokes/activates the license
- **Input**: `ClinicId`, `LicenseId`, `LicenceAdministrativeStatus` (Active|Suspended|Revoked)
- **Output**: `Success`

### 9. Add Department
- **Reason**: Clinic creates a new department
- **Input**: `ClinicId`, `Name`, `Code`, `ParentId?`
- **Output**: `DepartmentId` (Guid)

### 10. Update Department
- **Reason**: Department name/code/parent changes
- **Input**: `ClinicId`, `DepartmentId`, `Name`, `Code`, `ParentId?`
- **Output**: `Success`

### 11. Activate/Deactivate Department
- **Reason**: Temporarily close or reopen a department
- **Input**: `ClinicId`, `DepartmentId`, `IsActive`
- **Output**: `Success`

### 12. Add Specialty Link
- **Reason**: Clinic starts offering a new specialty
- **Input**: `ClinicId`, `SpecialtyId`, `ConsultationFee?`
- **Output**: `Success`

### 13. Update Specialty Link
- **Reason**: Consultation fee or active status changes for a linked specialty
- **Input**: `ClinicId`, `SpecialtyId`, `ConsultationFee?`, `IsActive`
- **Output**: `Success`

### 14. Reallocate Ownership
- **Reason**: Complete ownership restructure (replaces all allocations, enforces 100% invariant)
- **Input**: `ClinicId`, `ClinicOwnerShip[]` (OwnerId, OwnerShipType, SharePercentage, OwnershipEffectiveRange), `UserId`, `Timestamp`
- **Output**: `Success`

### 15. Transfer Ownership Percentage
- **Reason**: An owner reduces their share, distributing it among remaining owners
- **Input**: `ClinicId`, `SourceOwnerId`, `TargetAllocations[]` (OwnerId, decimal percentage), `SourceReductionAmount`, `UserId`, `Timestamp`
- **Output**: `Success`

### 16. Update Owner Share
- **Reason**: Adjust a single owner's share percentage
- **Input**: `ClinicId`, `OwnerShipId`, `NewSharePercentage`, `UserId`, `Timestamp`
- **Output**: `Success`

### 17. Expire Ownership
- **Reason**: Ownership period naturally ends
- **Input**: `ClinicId`, `OwnerShipId`, `UserId`, `Timestamp`
- **Output**: `Success`

### 18. Terminate Ownership
- **Reason**: Force-remove an owner (relinquished/terminated)
- **Input**: `ClinicId`, `OwnerShipId`, `UserId`, `Timestamp`
- **Output**: `Success`

---

## Queries (Read Side)

### 19. Get Clinic By Id
- **Reason**: View full clinic details
- **Input**: `ClinicId`
- **Output**: `ClinicDetailDto` (Info, Address, ContactInfo, Status, OperatingHours[], Departments[], Ownerships[], License, Specialties[])

### 20. Search Clinics
- **Reason**: Find clinics by name/status/city
- **Input**: `SearchTerm?`, `ClinicStatus?`, `City?`, `Page`, `PageSize`
- **Output**: `PaginatedResult<ClinicSummaryDto>` (Id, Name, City, Status, IsOperational)

### 21. Get Clinic Departments
- **Reason**: View hierarchical department structure
- **Input**: `ClinicId`
- **Output**: `DepartmentTreeDto[]` (Id, Name, Code, IsActive, Children[])

### 22. Get Clinic Ownerships
- **Reason**: View current and historical ownership structure
- **Input**: `ClinicId`
- **Output**: `OwnershipDto[]` (OwnerId, Type, SharePercentage, EffectivePeriod, Status)

### 23. Get Clinic License
- **Reason**: Verify clinic's license validity and operational status
- **Input**: `ClinicId`
- **Output**: `LicenseDto` (LicenseNumber, IssuingAuthority, Type, ValidityPeriod, AdministrativeStatus, IsOperational)

### 24. Get Clinic Operating Hours
- **Reason**: Know when the clinic is open
- **Input**: `ClinicId`
- **Output**: `OperatingHoursDto[]` (DayOfWeek, OpenTime?, CloseTime?, IsResting, IsOffDay)

---

## Proposed Domain Events

These events should be raised when aggregate state changes:

- `ClinicCreatedDomainEvent`
- `ClinicStatusChangedDomainEvent`
- `ClinicInfoUpdatedDomainEvent`
- `ClinicLicenseStatusChangedDomainEvent`
- `ClinicOwnershipReallocatedDomainEvent`
- `ClinicDepartmentAddedDomainEvent`
