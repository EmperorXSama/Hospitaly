# Doctor Aggregate Catalog

This catalog is focused on the `Doctor` aggregate and its child entities:
- `DoctorCredential` (1:many)
- `DoctorSpecialty` (1:many)
- `ClinicAffiliation` (1:many)

## Commands (Write Side)

### Implemented

1. **CreateDoctorCommand** ✅
   - **Reason**: Register a new doctor profile linked to a platform user.
   - **Input**: `UserId`
   - **Output**: `DoctorId`

2. **UpdateDoctorProfileCommand** ✅
   - **Reason**: Update doctor's professional profile fields (title, bio, avatar).
   - **Input**: `DoctorId`, `Title?`, `Bio?`, `AvatarUrl?`, `UserId`
   - **Output**: `Success`

3. **AddDoctorCredentialCommand** ✅
   - **Reason**: Submit a new professional credential (medical license, board certificate, malpractice insurance, degree).
   - **Input**: `DoctorId`, `CredentialType`, `IssuingAuthority`, `DocumentNumber`, `IssueDate`, `ExpiryDate`, `UserId`
   - **Output**: `CredentialId`

4. **VerifyDoctorCredentialCommand** ✅
   - **Reason**: Mark a credential as verified by an authorized user after document review.
   - **Input**: `DoctorId`, `CredentialId`, `UserId`
   - **Output**: `Success`

5. **UploadDoctorAvatarCommand** ✅
   - **Reason**: Set or change doctor's profile photo via a URL provided by BFF upload.
   - **Input**: `DoctorId`, `AvatarUrl`, `UserId`
   - **Output**: `Success`

6. **ActivateDoctorCommand** ✅
   - **Reason**: Activate a doctor's profile for clinical practice. Validates that all mandatory credentials are verified and valid.
   - **Input**: `DoctorId`, `UserId`
   - **Output**: `Success`

7. **DeactivateDoctorCommand** ✅
   - **Reason**: Temporarily suspend a doctor's ability to practice.
   - **Input**: `DoctorId`, `UserId`
   - **Output**: `Success`

8. **RevokeDoctorCredentialCommand** ✅
   - **Reason**: Permanently revoke a credential. This action is irreversible and the credential cannot be reactivated.
   - **Input**: `DoctorId`, `CredentialId`, `UserId`
   - **Output**: `Success`

9. **SuspendDoctorCredentialCommand** ✅
   - **Reason**: Temporarily suspend a credential's validity (not applicable to already revoked credentials).
   - **Input**: `DoctorId`, `CredentialId`, `UserId`
   - **Output**: `Success`

10. **ReactivateDoctorCredentialCommand** ✅
    - **Reason**: Reactivate a suspended credential. Revoked credentials cannot be reactivated.
    - **Input**: `DoctorId`, `CredentialId`, `UserId`
    - **Output**: `Success`

11. **AddDoctorSpecialtyCommand** ✅
    - **Reason**: Link one or more specialties to a doctor with certification details.
    - **Input**: `DoctorId`, `Specialties[]` (SpecialtyId, IsPrimary, CertificationNumber, CertifiedAt), `UserId`
    - **Output**: `Success`

12. **RemoveDoctorSpecialtyCommand** ✅
    - **Reason**: Unlink a specialty from the doctor's profile.
    - **Input**: `DoctorId`, `SpecialtyId`, `UserId`
    - **Output**: `Success`

13. **SetPrimaryDoctorSpecialtyCommand** ✅
    - **Reason**: Designate one specialty as the doctor's primary (demotes any existing primary).
    - **Input**: `DoctorId`, `SpecialtyId`, `UserId`
    - **Output**: `Success`

14. **AffiliateDoctorWithClinicCommand** ✅
    - **Reason**: Associate a doctor with a clinic, optionally assigning a department and granting clinical privileges.
    - **Input**: `DoctorId`, `ClinicId`, `JoinedDate`, `DepartmentId?`, `GrantedPrivileges?`, `UserId`
    - **Output**: `AffiliationId`

15. **ActivateClinicAffiliationCommand** ✅
    - **Reason**: Approve a pending clinic affiliation, moving it from pending to active status.
    - **Input**: `DoctorId`, `ClinicId`, `UserId`
    - **Output**: `Success`

16. **SuspendClinicAffiliationCommand** ✅
    - **Reason**: Temporarily suspend a doctor's practice at a clinic.
    - **Input**: `DoctorId`, `ClinicId`, `UserId`
    - **Output**: `Success`

17. **TerminateClinicAffiliationCommand**
    - **Reason**: End a doctor's association with a clinic permanently.
    - **Input**: `DoctorId`, `ClinicId`, `TerminatedDate`, `UserId`
    - **Output**: `Success`

18. **GrantDoctorClinicPrivilegeCommand**
    - **Reason**: Grant a specific clinical privilege (consult, prescribe, order labs, etc.) within a clinic affiliation.
    - **Input**: `DoctorId`, `ClinicId`, `PrivilegeType`, `UserId`
    - **Output**: `Success`

19. **RevokeDoctorClinicPrivilegeCommand**
    - **Reason**: Remove a previously granted privilege from a clinic affiliation.
    - **Input**: `DoctorId`, `ClinicId`, `PrivilegeType`, `UserId`
    - **Output**: `Success`

## Queries (Read Side)

20. **GetDoctorByIdQuery** ✅
    - **Reason**: Retrieve full doctor profile including specialties, credentials, and affiliations.
    - **Input**: `DoctorId`
    - **Output**: `DoctorDetailResponse`

21. **GetDoctorByUserIdQuery** ✅
    - **Reason**: Look up a doctor profile by the associated platform user ID.
    - **Input**: `UserId`
    - **Output**: `DoctorDetailResponse`

22. **SearchDoctorsQuery** ✅
    - **Reason**: Discover doctors by title, bio, specialty, clinic, or status with pagination.
    - **Input**: `SearchTerm?`, `SpecialtyId?`, `ClinicId?`, `Status?`, `Page`, `PageSize`
    - **Output**: `PaginatedResult<DoctorSummaryResponse>`

23. **GetDoctorCredentialsQuery** ✅
    - **Reason**: List all credentials for a doctor with their verification and validity status.
    - **Input**: `DoctorId`
    - **Output**: `List<DoctorCredentialResponse>`

24. **GetDoctorSpecialtiesQuery** ✅
    - **Reason**: Show declared specialties and indicate which is primary.
    - **Input**: `DoctorId`
    - **Output**: `List<DoctorSpecialtyResponse>`

25. **GetDoctorAffiliationsQuery** ✅
    - **Reason**: Show clinic affiliations with status, clinic name, department, and granted privileges.
    - **Input**: `DoctorId`
    - **Output**: `List<DoctorAffiliationResponse>`

26. **GetDoctorsByClinicQuery** ✅
    - **Reason**: List all doctors affiliated with a specific clinic, optionally filtered by affiliation status.
    - **Input**: `ClinicId`, `Status?`, `Page`, `PageSize`
    - **Output**: `PaginatedResult<DoctorAffiliationSummaryResponse>`
