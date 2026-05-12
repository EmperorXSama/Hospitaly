# Clinic Aggregate Catalog

This catalog is focused on the `Clinic` aggregate and its direct entities only:
- `OperatingLicense` (1:1)
- `Department` (1:many)
- `ClinicOwnerShip` (1:many)
- `ClinicSpecialty` (link entity for Clinic <-> Specialty)

## Commands (Write Side)

### Implemented

1. **CreateClinicCommand**
   - **Reason**: Onboard a new clinic with initial license, hours, and full ownership for creator.
   - **Input (DTO/Command)**: `UserId`, `Name`, `Description`, `Street`, `City`, `Region?`, `PostalCode?`, `Country`, `Phone?`, `Email?`
   - **Output**: `ClinicId`

### Planned (Clinic + child entities)

2. **UpdateClinicInfoCommand**
   - **Reason**: Edit clinic profile metadata.
   - **Input**: `ClinicId`, `Name`, `TradingName?`, `Description`, `LogoUrl?`, `UserId`
   - **Output**: `Success`

3. **UpdateClinicAddressCommand**
   - **Reason**: Relocate clinic or fix address details.
   - **Input**: `ClinicId`, `Street`, `City`, `Region?`, `PostalCode?`, `Country`, `Latitude?`, `Longitude?`, `UserId`
   - **Output**: `Success`

4. **UpdateClinicContactInfoCommand**
   - **Reason**: Update phone/email/website.
   - **Input**: `ClinicId`, `Phone?`, `Email?`, `Website?`, `UserId`
   - **Output**: `Success`

5. **SetClinicOperatingHoursCommand**
   - **Reason**: Define/edit weekly schedule.
   - **Input**: `ClinicId`, `OperatingHours[]`, `UserId`
   - **Output**: `Success`

6. **ReplaceOperatingLicenseCommand**
   - **Reason**: Add or replace operating license details.
   - **Input**: `ClinicId`, `LicenseNumber`, `IssuingAuthority`, `LicenseType`, `ValidityStart`, `ValidityEnd?`, `AdministrativeStatus`, `UserId`
   - **Output**: `Success`

7. **UpdateOperatingLicenseStatusCommand**
   - **Reason**: Activate/suspend/revoke license.
   - **Input**: `ClinicId`, `AdministrativeStatus`, `UserId`
   - **Output**: `Success`

8. **AddDepartmentCommand**
   - **Reason**: Add new department under clinic.
   - **Input**: `ClinicId`, `Name`, `Code`, `IsActive`, `ParentDepartmentId?`, `UserId`
   - **Output**: `DepartmentId`

9. **UpdateDepartmentCommand**
   - **Reason**: Rename/recode/re-parent department.
   - **Input**: `ClinicId`, `DepartmentId`, `Name`, `Code`, `ParentDepartmentId?`, `UserId`
   - **Output**: `Success`

10. **SetDepartmentActiveStateCommand**
    - **Reason**: Open/close department operationally.
    - **Input**: `ClinicId`, `DepartmentId`, `IsActive`, `UserId`
    - **Output**: `Success`

11. **AddClinicSpecialtyCommand**
    - **Reason**: Link clinic to a specialty offered.
    - **Input**: `ClinicId`, `SpecialtyId`, `IsActive`, `ConsultationFee?`, `UserId`
    - **Output**: `Success`

12. **UpdateClinicSpecialtyCommand**
    - **Reason**: Adjust specialty fee or state.
    - **Input**: `ClinicId`, `SpecialtyId`, `IsActive`, `ConsultationFee?`, `UserId`
    - **Output**: `Success`

13. **RemoveClinicSpecialtyCommand**
    - **Reason**: Unlink specialty from clinic offerings.
    - **Input**: `ClinicId`, `SpecialtyId`, `UserId`
    - **Output**: `Success`

14. **ReAllocateClinicOwnershipCommand**
    - **Reason**: Full ownership restructure while enforcing active-share total = 100%.
    - **Input**: `ClinicId`, `Owners[]` (`OwnershipId?`, `OwnerId`, `OwnerShipType`, `SharePercentage`, `EffectiveStart`, `EffectiveEnd?`, `Status`), `UserId`, `UpdatedOn`
    - **Output**: `Success`

15. **TransferClinicOwnershipPercentageCommand**
    - **Reason**: Move part of one owner share to one or many other owners.
    - **Input**: `ClinicId`, `FromOwnershipId`, `RetainedPercentage`, `Transfers[]` (`OwnershipId`, `SharePercentage`), `UserId`, `UpdatedOn`
    - **Output**: `Success`

16. **UpdateClinicOwnerShareCommand**
    - **Reason**: Adjust single owner percentage.
    - **Input**: `ClinicId`, `OwnershipId`, `NewSharePercentage`, `UserId`, `UpdatedOn`
    - **Output**: `Success`

17. **ExpireClinicOwnershipCommand**
    - **Reason**: Mark ownership expired after end date.
    - **Input**: `ClinicId`, `OwnershipId`, `UserId`, `UpdatedOn`
    - **Output**: `Success`

18. **TerminateClinicOwnershipCommand**
    - **Reason**: Force end active ownership.
    - **Input**: `ClinicId`, `OwnershipId`, `UserId`, `UpdatedOn`
    - **Output**: `Success`

19. **ApplyClinicOwnershipEndDateCommand**
    - **Reason**: Set or change ownership end date.
    - **Input**: `ClinicId`, `OwnershipId`, `EffectiveUntil`, `UserId`, `UpdatedOn`
    - **Output**: `Success`

## Queries (Read Side)

1. **GetClinicByIdQuery**
   - **Reason**: Read complete clinic aggregate view.
   - **Input**: `ClinicId`
   - **Output**: `ClinicDetailDto`

2. **SearchClinicsQuery**
   - **Reason**: Discover clinics by basic filters.
   - **Input**: `SearchTerm?`, `City?`, `Page`, `PageSize`
   - **Output**: `PaginatedResult<ClinicSummaryDto>`

3. **GetClinicDepartmentsQuery**
   - **Reason**: Show department hierarchy.
   - **Input**: `ClinicId`
   - **Output**: `DepartmentTreeDto[]`

4. **GetClinicOwnershipsQuery**
   - **Reason**: Show active/history ownership allocations.
   - **Input**: `ClinicId`
   - **Output**: `ClinicOwnershipDto[]`

5. **GetClinicSpecialtiesQuery**
   - **Reason**: Show linked specialties and fees.
   - **Input**: `ClinicId`
   - **Output**: `ClinicSpecialtyDto[]`

6. **GetClinicOperatingLicenseQuery**
   - **Reason**: Validate legal/operational license state.
   - **Input**: `ClinicId`
   - **Output**: `OperatingLicenseDto`

7. **GetClinicOperatingHoursQuery**
   - **Reason**: Show opening schedule.
   - **Input**: `ClinicId`
   - **Output**: `OperatingHoursDto[]`
