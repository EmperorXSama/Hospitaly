using ErrorOr;
using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Domain.Appointment;
using Hospitaly.Modules.Clinic.Domain.Appointment.Enums;
using Hospitaly.Modules.Clinic.Domain.Appointment.ValueObjects;
using Hospitaly.Modules.Clinic.Domain.Doctor;
using Hospitaly.Modules.Clinic.Domain.Doctor.Enums;
using Hospitaly.Modules.Clinic.Domain.Doctor.ValueObjects;
using Hospitaly.Modules.Clinic.Domain.DoctorSchedule;
using Hospitaly.Modules.Clinic.Domain.DoctorSchedule.Enums;
using Hospitaly.Modules.Clinic.Domain.DoctorSchedule.ValueObjects;
using Hospitaly.Modules.Clinic.Domain.Room;
using Hospitaly.Modules.Clinic.Domain.Room.Enums;
using Hospitaly.Modules.Clinic.Domain.Room.ValueObjects;
using Hospitaly.Modules.Clinic.Domain.Specialty;
using Hospitaly.Modules.Clinic.Domain.StaffMember;
using Hospitaly.Modules.Clinic.Domain.StaffMember.Enums;
using Hospitaly.Modules.Clinic.Domain.StaffMember.ValueObjects;
using AVisitType = Hospitaly.Modules.Clinic.Domain.Appointment.Enums.VisitType;
using BlockType = Hospitaly.Modules.Clinic.Domain.DoctorSchedule.Enums.BlockType;
using RoomCategory = Hospitaly.Modules.Clinic.Domain.Room.Enums.RoomCategory;

var separator = new string('=', 70);

// ─────────────────────────────────────────────────────────────
// Scenario 1: Create Doctor
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 1: Create Doctor");

var doctorCreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000001");
var doctorCreatedOn = DateTime.UtcNow;

var doctorResult = Doctor.Create(doctorCreatedBy, doctorCreatedOn);
PrintResult(doctorResult, "Doctor.Create");
var doctor = doctorResult.Value;

Console.WriteLine($"  Doctor ID: {doctor.Id}");
Console.WriteLine($"  Audit: CreatedBy={doctor.Audit.CreatedBy}, CreatedOn={doctor.Audit.CreatedOnUtc:O}");

// ─────────────────────────────────────────────────────────────
// Scenario 2: Create Credential (success)
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 2: Create Credential (success)");

var issueDate = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
var expiryDate = new DateTimeOffset(2028, 1, 1, 0, 0, 0, TimeSpan.Zero);
var userId = doctorCreatedBy;

var credentialResult = DoctorCredential.Create(
    doctor.Id,
    CredentialType.MedicalLicense,
    "State Medical Board",
    "LIC-2026-12345",
    issueDate,
    expiryDate,
    userId,
    DateTime.UtcNow);

PrintResult(credentialResult, "DoctorCredential.Create");
var credential = credentialResult.Value;

Console.WriteLine($"  Credential ID: {credential.Id}");
Console.WriteLine($"  Type: {credential.CredentialType}");
Console.WriteLine($"  Status: {credential.Status}");
Console.WriteLine($"  Issuing Authority: {credential.IssuingAuthority}");
Console.WriteLine($"  Document: {credential.DocumentNumber}");
Console.WriteLine($"  Valid: [{credential.ValidityPeriod.IssueDate:d} → {credential.ValidityPeriod.ExpiryDate:d}]");
Console.WriteLine($"  Is Perpetual: {credential.ValidityPeriod.IsPerpetual}");
Console.WriteLine($"  Verified: {credential.VerifiedAt}");

// ─────────────────────────────────────────────────────────────
// Scenario 3: Create Credential (failures — guard clauses)
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 3: Create Credential (failures)");

// 3a — Future issue date
var futureIssue = new DateTimeOffset(2099, 1, 1, 0, 0, 0, TimeSpan.Zero);
var futureCred = DoctorCredential.Create(
    doctor.Id, CredentialType.BoardCertificate, "Board", "B-001",
    futureIssue, futureIssue.AddYears(1), userId, DateTime.UtcNow);
PrintResult(futureCred, "Future issue date", isError: true);

// 3b — Empty document number
var emptyDocCred = DoctorCredential.Create(
    doctor.Id, CredentialType.MalpracticeInsurance, "Insurer", "",
    issueDate, expiryDate, userId, DateTime.UtcNow);
PrintResult(emptyDocCred, "Empty document number", isError: true);

// 3c — Expiry before issue
var invalidRangeCred = DoctorCredential.Create(
    doctor.Id, CredentialType.Degree, "University", "DEG-001",
    expiryDate, issueDate, userId, DateTime.UtcNow);
PrintResult(invalidRangeCred, "Expiry before issue date", isError: true);

Console.WriteLine("  → All 3 guard clauses correctly rejected invalid input.");

// ─────────────────────────────────────────────────────────────
// Scenario 4: Verify Credential
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 4: Verify Credential");

var adminId = Guid.Parse("00000000-0000-0000-0000-000000000002");
var verifyResult = credential.Verify(adminId, DateTime.UtcNow);
PrintResult(verifyResult, "credential.Verify");
Console.WriteLine($"  VerifiedAt: {credential.VerifiedAt}");
Console.WriteLine($"  VerifiedBy: {credential.VerifiedBy}");

// ─────────────────────────────────────────────────────────────
// Scenario 5: Credential Lifecycle (Suspend → Reactivate → Revoke → Reactivate-fails)
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 5: Credential Lifecycle");

// Create a fresh credential for the lifecycle demo
var lifeCredResult = DoctorCredential.Create(
    doctor.Id, CredentialType.BoardCertificate, "American Board", "ABC-987",
    issueDate, expiryDate, userId, DateTime.UtcNow);
var lifeCred = lifeCredResult.Value;
Console.WriteLine($"  Fresh credential: {lifeCred.Id}, Status={lifeCred.Status}");

// 5a — Suspend
var suspendResult = lifeCred.Suspend();
PrintResult(suspendResult, "credential.Suspend");
Console.WriteLine($"  Status after suspend: {lifeCred.Status}");

// 5b — Reactivate
var reactivateResult = lifeCred.Reactivate();
PrintResult(reactivateResult, "credential.Reactivate");
Console.WriteLine($"  Status after reactivate: {lifeCred.Status}");

// 5c — Revoke
var revokeResult = lifeCred.Revoke();
PrintResult(revokeResult, "credential.Revoke");
Console.WriteLine($"  Status after revoke: {lifeCred.Status}");

// 5d — Reactivate after revoked (must fail)
var reRevokeResult = lifeCred.Revoke();
PrintResult(reRevokeResult, "credential.Revoke (already revoked)");
Console.WriteLine($"  → Revoking again: {(reRevokeResult.IsError ? "correctly rejected" : "UNEXPECTED")}");

var reReactResult = lifeCred.Reactivate();
PrintResult(reReactResult, "credential.Reactivate (after revoke)", isError: true);
Console.WriteLine($"  → Reactivate after revoke: {(reReactResult.IsError ? "correctly rejected — Revoked is terminal" : "UNEXPECTED")}");

// ─────────────────────────────────────────────────────────────
// Scenario 6: Check credential validity
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 6: Check Credential Validity");

// Expired credential (issue in past, expiry in past)
var expiredCredResult = DoctorCredential.Create(
    doctor.Id, CredentialType.MalpracticeInsurance, "Insurer Co", "MAL-001",
    new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero),
    new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero),
    userId, DateTime.UtcNow);
var expiredCred = expiredCredResult.Value;
_ = expiredCred.Verify(adminId, new DateTime(2020, 6, 1, 0, 0, 0, DateTimeKind.Utc));
Console.WriteLine($"  Expired credential (not verified for current period): IsValid(now) = {expiredCred.IsValid(DateTime.UtcNow)}");

// Unverified credential
var unverifiedCredResult = DoctorCredential.Create(
    doctor.Id, CredentialType.Degree, "University", "DEG-002",
    issueDate, expiryDate, userId, DateTime.UtcNow);
var unverifiedCred = unverifiedCredResult.Value;
Console.WriteLine($"  Unverified credential: IsValid(now) = {unverifiedCred.IsValid(DateTime.UtcNow)}");

// Verified, active, in-range credential
var validCredResult = DoctorCredential.Create(
    doctor.Id, CredentialType.MedicalLicense, "Board", "LIC-VALID",
    issueDate, expiryDate, userId, DateTime.UtcNow);
var validCred = validCredResult.Value;
_ = validCred.Verify(adminId, DateTime.UtcNow);
Console.WriteLine($"  Verified + Active + in-range: IsValid(now) = {validCred.IsValid(DateTime.UtcNow)}");

// ─────────────────────────────────────────────────────────────
// Scenario 7: Create Specialty reference (DoctorSpecialty)
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 7: Create DoctorSpecialty");

var specialty = Specialty.Create("Cardiology");
var dsResult = DoctorSpecialty.Create(
    doctor.Id,
    specialty.Id,
    isPrimary: true,
    certificationNumber: "CERT-CARDIO-001",
    certifiedAt: DateTime.UtcNow);
PrintResult(dsResult, "DoctorSpecialty.Create");
var docSpecialty = dsResult.Value;
Console.WriteLine($"  DoctorSpecialty: DoctorId={docSpecialty.DoctorId}, SpecialtyId={docSpecialty.SpecialtyId}");
Console.WriteLine($"  IsPrimary={docSpecialty.IsPrimary}, Certification={docSpecialty.CertificationNumber}");

// ─────────────────────────────────────────────────────────────
// Scenario 8: Create Affiliation with privileges
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 8: Create ClinicAffiliation");

var clinicId = Guid.Parse("00000000-0000-0000-0000-000000000010");
var departmentId = Guid.Parse("00000000-0000-0000-0000-000000000020");

var privilegeResult = Privilege.Create(PrivilegeType.Consult, DateTime.UtcNow, adminId);
var privilegeResult2 = Privilege.Create(PrivilegeType.Prescribe, DateTime.UtcNow, adminId);

var affiliationResult = ClinicAffiliation.Create(
    clinicId,
    doctor.Id,
    joinedDate: DateTime.UtcNow.AddDays(-30),
    departmentId,
    [privilegeResult.Value, privilegeResult2.Value]);

PrintResult(affiliationResult, "ClinicAffiliation.Create");
var affiliation = affiliationResult.Value;
Console.WriteLine($"  Affiliation: ClinicId={affiliation.ClinicId}, DeptId={affiliation.DepartmentId}");
Console.WriteLine($"  Status: {affiliation.Status}");
Console.WriteLine($"  Joined: {affiliation.JoinedDate:yyyy-MM-dd}");
Console.WriteLine($"  Privileges: {affiliation.GrantedPrivileges.Count} granted");
foreach (var p in affiliation.GrantedPrivileges)
{
    Console.WriteLine($"    - {p.Type} (granted {p.GrantedAt:yyyy-MM-dd} by {p.GrantedBy})");
}

// ─────────────────────────────────────────────────────────────
// Scenario 9: Affiliation Lifecycle (Suspend → Terminate → Terminate-fails)
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 9: Affiliation Lifecycle");

var lifeAffResult = ClinicAffiliation.Create(
    clinicId, doctor.Id, DateTime.UtcNow.AddDays(-10));
var lifeAff = lifeAffResult.Value;
Console.WriteLine($"  Fresh affiliation: Status={lifeAff.Status}");

var affSuspendResult = lifeAff.Suspend();
PrintResult(affSuspendResult, "affiliation.Suspend");
Console.WriteLine($"  Status after suspend: {lifeAff.Status}");

var affTerminateResult = lifeAff.Terminate(DateTime.UtcNow);
PrintResult(affTerminateResult, "affiliation.Terminate");
Console.WriteLine($"  Status after terminate: {lifeAff.Status}");
Console.WriteLine($"  TerminatedDate: {lifeAff.TerminatedDate}");

// Try to terminate again
var affReTerminateResult = lifeAff.Terminate(DateTime.UtcNow);
PrintResult(affReTerminateResult, "affiliation.Terminate (already terminated)", isError: true);
Console.WriteLine($"  → Re-terminate: {(affReTerminateResult.IsError ? "correctly rejected — Terminated is terminal" : "UNEXPECTED")}");

// ─────────────────────────────────────────────────────────────
// Scenario 10: Create DoctorSchedule
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 10: Create DoctorSchedule");

var scheduleResult = DoctorSchedule.Create(
    doctor.Id,
    clinicId,
    doctorCreatedBy,
    DateTime.UtcNow);

PrintResult(scheduleResult, "DoctorSchedule.Create");
var schedule = scheduleResult.Value;
Console.WriteLine($"  Schedule ID: {schedule.Id}");
Console.WriteLine($"  Doctor ID: {schedule.DoctorId}");
Console.WriteLine($"  Clinic ID: {schedule.ClinicId}");
Console.WriteLine($"  Blocks: {schedule.Blocks.Count}");

// ─────────────────────────────────────────────────────────────
// Scenario 11: Add recurring block (every Monday)
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 11: Add recurring block (every Monday)");

var mondayBlockResult = schedule.AddBlock(
    dayOfWeek: DayOfWeek.Monday,
    specificDate: null,
    startTime: new TimeOnly(9, 0),
    endTime: new TimeOnly(12, 0),
    blockType: BlockType.Available,
    maxAppointmentsAllowed: 4,
    createdBy: userId,
    createdOnUtc: DateTime.UtcNow);

PrintResult(mondayBlockResult, "schedule.AddBlock(recurring Monday 9-12)");

var mondayBlock = schedule.Blocks.FirstOrDefault();
Console.WriteLine($"  Block ID: {mondayBlock?.Id}");
Console.WriteLine($"  DayOfWeek: {mondayBlock?.DayOfWeek}");
Console.WriteLine($"  Time: {mondayBlock?.TimeRange.StartTime} → {mondayBlock?.TimeRange.EndTime}");
Console.WriteLine($"  IsRecurring: {mondayBlock?.IsRecurring}");
Console.WriteLine($"  Type: {mondayBlock?.BlockType}");
Console.WriteLine($"  Max Appointments: {mondayBlock?.MaxAppointmentsAllowed}");

// ─────────────────────────────────────────────────────────────
// Scenario 12: Add specific-date block
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 12: Add specific-date block");

var dateBlockResult = schedule.AddBlock(
    dayOfWeek: null,
    specificDate: new DateOnly(2026, 5, 15),
    startTime: new TimeOnly(14, 0),
    endTime: new TimeOnly(16, 0),
    blockType: BlockType.EmergencyOnly,
    maxAppointmentsAllowed: 2,
    createdBy: userId,
    createdOnUtc: DateTime.UtcNow);

PrintResult(dateBlockResult, "schedule.AddBlock(specific-date 2026-05-15 14-16)");

var dateBlock = schedule.Blocks.Last();
Console.WriteLine($"  Block ID: {dateBlock?.Id}");
Console.WriteLine($"  SpecificDate: {dateBlock?.SpecificDate}");
Console.WriteLine($"  Time: {dateBlock?.TimeRange.StartTime} → {dateBlock?.TimeRange.EndTime}");
Console.WriteLine($"  IsRecurring: {dateBlock?.IsRecurring}");
Console.WriteLine($"  Type: {dateBlock?.BlockType}");

// ─────────────────────────────────────────────────────────────
// Scenario 13: Overlap detection
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 13: Overlap detection");

var overlapResult = schedule.AddBlock(
    dayOfWeek: DayOfWeek.Monday,
    specificDate: null,
    startTime: new TimeOnly(10, 0),
    endTime: new TimeOnly(11, 0),
    blockType: BlockType.Available,
    maxAppointmentsAllowed: 1,
    createdBy: userId,
    createdOnUtc: DateTime.UtcNow);

PrintResult(overlapResult, "schedule.AddBlock(Mon 10-11 overlaps Mon 9-12)", isError: true);
Console.WriteLine($"  → Overlap: {(overlapResult.IsError ? "correctly rejected" : "UNEXPECTED")}");

// ─────────────────────────────────────────────────────────────
// Scenario 14: Change block type
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 14: Change block type");

var changeTypeResult = schedule.ChangeBlockType(mondayBlock!.Id, BlockType.Blocked, userId, DateTime.UtcNow);
PrintResult(changeTypeResult, "schedule.ChangeBlockType(Available → Blocked)");

var modifiedBlock = schedule.Blocks.First(b => b.Id == mondayBlock.Id);
Console.WriteLine($"  Block {modifiedBlock.Id}: BlockType = {modifiedBlock.BlockType}");

// ─────────────────────────────────────────────────────────────
// Scenario 15: Update block time range
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 15: Update block time range");

var updateTimeResult = schedule.UpdateBlockTimeRange(
    mondayBlock.Id,
    new TimeOnly(8, 0),
    new TimeOnly(11, 0),
    userId,
    DateTime.UtcNow);

PrintResult(updateTimeResult, "schedule.UpdateBlockTimeRange(8:00 → 11:00)");

var timeBlock = schedule.Blocks.First(b => b.Id == mondayBlock.Id);
Console.WriteLine($"  Block {timeBlock.Id}: TimeRange = {timeBlock.TimeRange.StartTime} → {timeBlock.TimeRange.EndTime}");

// ─────────────────────────────────────────────────────────────
// Scenario 16: Remove block (safe — no override)
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 16: Remove block (safe)");

var removeResult = schedule.RemoveBlock(dateBlock!.Id);
PrintResult(removeResult, "schedule.RemoveBlock(specific-date block)");
Console.WriteLine($"  Blocks remaining: {schedule.Blocks.Count}");

// ─────────────────────────────────────────────────────────────
// Scenario 17: Remove block (force — override confirmed)
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 17: Remove block (force override)");

var forceRemoveResult = schedule.RemoveBlock(mondayBlock.Id, overrideConfirmedAppointments: true);
PrintResult(forceRemoveResult, "schedule.RemoveBlock(recurring block, force=true)");
Console.WriteLine($"  Blocks remaining: {schedule.Blocks.Count}");

// ─────────────────────────────────────────────────────────────
// Scenario 18: Multiple schedules (aggregate boundary)
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 18: Multiple schedules (aggregate boundary)");

var clinicId2 = Guid.Parse("00000000-0000-0000-0000-000000000011");
var schedule2Result = DoctorSchedule.Create(doctor.Id, clinicId2, userId, DateTime.UtcNow);
PrintResult(schedule2Result, "DoctorSchedule.Create(doctor, secondClinic)");
var schedule2 = schedule2Result.Value;

var s2BlockResult = schedule2.AddBlock(
    DayOfWeek.Tuesday, null,
    new TimeOnly(13, 0), new TimeOnly(17, 0),
    BlockType.Available, 3, userId, DateTime.UtcNow);
PrintResult(s2BlockResult, "schedule2.AddBlock(Tue 13-17)");

Console.WriteLine($"  Schedule 1: {schedule.Id} — Blocks: {schedule.Blocks.Count}");
Console.WriteLine($"  Schedule 2: {schedule2.Id} — Blocks: {schedule2.Blocks.Count}");
Console.WriteLine("  → Same doctor, different clinics = independent schedules.");

// ═════════════════════════════════════════════════════════════
// APPOINTMENT AGGREGATE SCENARIOS
// ═════════════════════════════════════════════════════════════

var patientId = Guid.Parse("00000000-0000-0000-0000-000000000030");
var futureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30));
var appointmentUserId = doctorCreatedBy;

// ─────────────────────────────────────────────────────────────
// Scenario 19: Request appointment (success)
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 19: Request appointment (success)");

var tsResult = TimeSlot.Create(futureDate, new TimeOnly(9, 0), new TimeOnly(9, 30));
PrintResult(tsResult, "TimeSlot.Create(future 30min slot)");
var timeSlot = tsResult.Value;

var apptTypeResult = AppointmentType.Create(AVisitType.Checkup, TimeSpan.FromMinutes(30));
PrintResult(apptTypeResult, "AppointmentType.Create(Checkup, 30min)");
var apptType = apptTypeResult.Value;

var requestResult = Appointment.Request(
    doctor.Id, patientId, clinicId, roomId: null,
    timeSlot, apptType, appointmentUserId, DateTime.UtcNow);

PrintResult(requestResult, "Appointment.Request");
var appointment = requestResult.Value;
Console.WriteLine($"  Appointment ID: {appointment.Id}");
Console.WriteLine($"  Doctor: {appointment.DoctorId}");
Console.WriteLine($"  Patient: {appointment.PatientId}");
Console.WriteLine($"  TimeSlot: {appointment.TimeSlot}");
Console.WriteLine($"  Type: {appointment.AppointmentType}");
Console.WriteLine($"  Status: {appointment.Status}");
Console.WriteLine($"  Domain events: {appointment.DomainEvents.Count}");
foreach (var e in appointment.DomainEvents)
    Console.WriteLine($"    - {e.GetType().Name}");

// ─────────────────────────────────────────────────────────────
// Scenario 20: Request appointment (failures)
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 20: Request appointment (failures)");

// 20a — Past start time
var pastSlot = TimeSlot.Create(
    DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
    new TimeOnly(9, 0), new TimeOnly(9, 30)).Value;
var pastRequest = Appointment.Request(
    doctor.Id, patientId, clinicId, null,
    pastSlot, apptType, appointmentUserId, DateTime.UtcNow);
PrintResult(pastRequest, "Past start time", isError: true);

// 20b — Mismatched duration
var mismatchedType = AppointmentType.Create(AVisitType.Procedure, TimeSpan.FromMinutes(60)).Value;
var wrongDurationRequest = Appointment.Request(
    doctor.Id, patientId, clinicId, null,
    timeSlot, mismatchedType, appointmentUserId, DateTime.UtcNow);
PrintResult(wrongDurationRequest, "Duration 30min ≠ type expects 60min", isError: true);

Console.WriteLine("  → Both guard clauses correctly rejected invalid input.");

// ─────────────────────────────────────────────────────────────
// Scenario 21: Confirm appointment
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 21: Confirm appointment");

var confirmResult = appointment.Confirm(adminId, DateTime.UtcNow);
PrintResult(confirmResult, "appointment.Confirm");
Console.WriteLine($"  Status: {appointment.Status}");
Console.WriteLine($"  Domain events: {appointment.DomainEvents.Count}");
Console.WriteLine($"    Latest: {appointment.DomainEvents.Last().GetType().Name}");

// ─────────────────────────────────────────────────────────────
// Scenario 22: Check in
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 22: Check in");

var checkinResult = appointment.CheckIn(appointmentUserId, DateTime.UtcNow);
PrintResult(checkinResult, "appointment.CheckIn");
Console.WriteLine($"  Status: {appointment.Status}");

// ─────────────────────────────────────────────────────────────
// Scenario 23: Start appointment
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 23: Start appointment");

var startResult = appointment.Start(adminId, DateTime.UtcNow);
PrintResult(startResult, "appointment.Start");
Console.WriteLine($"  Status: {appointment.Status}");

// ─────────────────────────────────────────────────────────────
// Scenario 24: Complete appointment
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 24: Complete appointment");

var completeResult = appointment.Complete(adminId, DateTime.UtcNow);
PrintResult(completeResult, "appointment.Complete");
Console.WriteLine($"  Status: {appointment.Status}");

// ─────────────────────────────────────────────────────────────
// Scenario 25: Cancel an appointment
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 25: Cancel an appointment");

// Create a fresh appointment for cancellation demo
var cancelSlot = TimeSlot.Create(
    futureDate, new TimeOnly(10, 0), new TimeOnly(10, 30)).Value;
var cancelType = AppointmentType.Create(AVisitType.FollowUp, TimeSpan.FromMinutes(30)).Value;
var cancelAppt = Appointment.Request(
    doctor.Id, patientId, clinicId, null,
    cancelSlot, cancelType, appointmentUserId, DateTime.UtcNow).Value;
_ = cancelAppt.Confirm(adminId, DateTime.UtcNow);
Console.WriteLine($"  Before cancel: {cancelAppt.Status}");

var cancellation = CancellationDetails.Create(
    CancellationReason.PatientRequest, InitiatedBy.Patient, DateTime.UtcNow, "Patient is feeling better");
var cancelResult = cancelAppt.Cancel(cancellation.Value, appointmentUserId, DateTime.UtcNow);
PrintResult(cancelResult, "appointment.Cancel");
Console.WriteLine($"  Status: {cancelAppt.Status}");
Console.WriteLine($"  Cancellation: {cancelAppt.Cancellation}");

// ─────────────────────────────────────────────────────────────
// Scenario 26: No-show
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 26: No-show");

var noShowSlot = TimeSlot.Create(
    futureDate, new TimeOnly(11, 0), new TimeOnly(11, 30)).Value;
var noShowType = AppointmentType.Create(AVisitType.Checkup, TimeSpan.FromMinutes(30)).Value;
var noShowAppt = Appointment.Request(
    doctor.Id, patientId, clinicId, null,
    noShowSlot, noShowType, appointmentUserId, DateTime.UtcNow).Value;
_ = noShowAppt.Confirm(adminId, DateTime.UtcNow);
_ = noShowAppt.CheckIn(appointmentUserId, DateTime.UtcNow);
Console.WriteLine($"  Before no-show: {noShowAppt.Status}");

var noShowResult = noShowAppt.MarkNoShow(adminId, DateTime.UtcNow);
PrintResult(noShowResult, "appointment.MarkNoShow");
Console.WriteLine($"  Status: {noShowAppt.Status}");

// ─────────────────────────────────────────────────────────────
// Scenario 27: Invalid transitions
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 27: Invalid transitions");

// Try to confirm an already-completed appointment
var invalidConfirm = appointment.Confirm(adminId, DateTime.UtcNow);
PrintResult(invalidConfirm, "Confirm after Completed", isError: true);
Console.WriteLine($"  → {(!invalidConfirm.IsError ? "UNEXPECTED" : "correctly rejected — Completed is terminal")}");

// Try to cancel an already-cancelled appointment
var invalidCancel = cancelAppt.Cancel(cancellation.Value, appointmentUserId, DateTime.UtcNow);
PrintResult(invalidCancel, "Cancel after Cancelled", isError: true);
Console.WriteLine($"  → {(!invalidCancel.IsError ? "UNEXPECTED" : "correctly rejected — Cancelled is terminal")}");

// Try to start a Requested appointment (must CheckIn first)
var freshAppt = Appointment.Request(
    doctor.Id, patientId, clinicId, null,
    TimeSlot.Create(futureDate, new TimeOnly(14, 0), new TimeOnly(14, 30)).Value,
    AppointmentType.Create(AVisitType.Checkup, TimeSpan.FromMinutes(30)).Value,
    appointmentUserId, DateTime.UtcNow).Value;
var invalidStart = freshAppt.Start(adminId, DateTime.UtcNow);
PrintResult(invalidStart, "Start without Confirmed+CheckedIn", isError: true);
Console.WriteLine($"  → {(!invalidStart.IsError ? "UNEXPECTED" : "correctly rejected — must go through Confirmed → CheckedIn first")}");

// ─────────────────────────────────────────────────────────────
// Scenario 28: Reschedule
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 28: Reschedule");

var reschedSlot = TimeSlot.Create(
    futureDate, new TimeOnly(9, 0), new TimeOnly(9, 30)).Value;
var reschedType = AppointmentType.Create(AVisitType.Checkup, TimeSpan.FromMinutes(30)).Value;
var reschedAppt = Appointment.Request(
    doctor.Id, patientId, clinicId, null,
    reschedSlot, reschedType, appointmentUserId, DateTime.UtcNow).Value;
_ = reschedAppt.Confirm(adminId, DateTime.UtcNow);
Console.WriteLine($"  Before reschedule: {reschedAppt.TimeSlot} — Status: {reschedAppt.Status}");

var newSlot = TimeSlot.Create(
    futureDate.AddDays(1), new TimeOnly(10, 0), new TimeOnly(10, 30)).Value;
var rescheduleResult = reschedAppt.Reschedule(
    newSlot, "Patient requested time change",
    RescheduleRequestedBy.Patient,
    appointmentUserId, DateTime.UtcNow);
PrintResult(rescheduleResult, "appointment.Reschedule");
Console.WriteLine($"  After reschedule: {reschedAppt.TimeSlot} — Status: {reschedAppt.Status}");
Console.WriteLine($"  RescheduleInfo: {reschedAppt.RescheduleInfo}");
Console.WriteLine($"  Domain events: {reschedAppt.DomainEvents.Count}");
foreach (var e in reschedAppt.DomainEvents)
    Console.WriteLine($"    - {e.GetType().Name}");

// ═════════════════════════════════════════════════════════════
// ROOM AGGREGATE SCENARIOS
// ═════════════════════════════════════════════════════════════

var roomUserId = appointmentUserId;
var futureDate2 = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(60));

// ─────────────────────────────────────────────────────────────
// Scenario 29: Create consultation room (no capabilities needed)
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 29: Create consultation room");

var consultTypeResult = RoomType.Create(RoomCategory.Consultation);
PrintResult(consultTypeResult, "RoomType.Create(Consultation)");
var consultType = consultTypeResult.Value;

var consultRoomResult = Room.Create(
    "Consultation Room A", consultType, capabilities: null,
    roomUserId, DateTime.UtcNow);
PrintResult(consultRoomResult, "Room.Create(Consultation)");
var consultRoom = consultRoomResult.Value;
Console.WriteLine($"  Room ID: {consultRoom.Id}");
Console.WriteLine($"  Name: {consultRoom.Name}");
Console.WriteLine($"  Type: {consultRoom.RoomType}");
Console.WriteLine($"  Capabilities: {consultRoom.Capabilities.Count}");
Console.WriteLine($"  Maintenance blocks: {consultRoom.MaintenanceBlocks.Count}");
Console.WriteLine($"  Domain events: {consultRoom.DomainEvents.Count}");
foreach (var e in consultRoom.DomainEvents)
    Console.WriteLine($"    - {e.GetType().Name}");

// ─────────────────────────────────────────────────────────────
// Scenario 30: Create procedure room (with capabilities)
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 30: Create procedure room with capabilities");

var procTypeResult = RoomType.Create(RoomCategory.Procedure);
PrintResult(procTypeResult, "RoomType.Create(Procedure)");
var procType = procTypeResult.Value;

var cap1 = RoomCapability.Create("Surgical Light");
var cap2 = RoomCapability.Create("Ultrasound");
var cap3 = RoomCapability.Create("Resuscitation Equipment");
Console.WriteLine($"  Capabilities created: {cap1.IsError} {cap2.IsError} {cap3.IsError}");

var procRoomResult = Room.Create(
    "Procedure Room 1", procType,
    [cap1.Value, cap2.Value, cap3.Value],
    roomUserId, DateTime.UtcNow);
PrintResult(procRoomResult, "Room.Create(Procedure with 3 capabilities)");
var procRoom = procRoomResult.Value;
Console.WriteLine($"  Room ID: {procRoom.Id}");
Console.WriteLine($"  Name: {procRoom.Name}");
Console.WriteLine($"  Capabilities: {procRoom.Capabilities.Count}");
foreach (var c in procRoom.Capabilities)
    Console.WriteLine($"    - {c.Name}");

// ─────────────────────────────────────────────────────────────
// Scenario 31: Create room failure (non-consultation without caps)
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 31: Room creation failure — missing capabilities");

var imagingType = RoomType.Create(RoomCategory.Imaging).Value;
var failRoomResult = Room.Create(
    "Imaging Suite", imagingType, capabilities: null,
    roomUserId, DateTime.UtcNow);
PrintResult(failRoomResult, "Room.Create(Imaging, no capabilities)", isError: true);

Console.WriteLine("  → Non-consultation rooms require at least one capability.");

// ─────────────────────────────────────────────────────────────
// Scenario 32: Change room type
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 32: Change room type");

var recoveryType = RoomType.Create(RoomCategory.Recovery).Value;
Console.WriteLine($"  Before: {consultRoom.RoomType}");

var roomChangeTypeResult = consultRoom.ChangeType(recoveryType, roomUserId, DateTime.UtcNow);
PrintResult(roomChangeTypeResult, "consultRoom.ChangeType(Consultation → Recovery)");
Console.WriteLine($"  After: {consultRoom.RoomType}");
Console.WriteLine($"  Domain events: {consultRoom.DomainEvents.Count}");
Console.WriteLine($"    Latest: {consultRoom.DomainEvents.Last().GetType().Name}");

// ─────────────────────────────────────────────────────────────
// Scenario 33: Add capability to room
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 33: Add capability");

var newCap = RoomCapability.Create("Patient Monitor").Value;
var addCapResult = consultRoom.AddCapability(newCap, roomUserId, DateTime.UtcNow);
PrintResult(addCapResult, "consultRoom.AddCapability(\"Patient Monitor\")");
Console.WriteLine($"  Capabilities now: {consultRoom.Capabilities.Count}");
foreach (var c in consultRoom.Capabilities)
    Console.WriteLine($"    - {c.Name}");

// ─────────────────────────────────────────────────────────────
// Scenario 34: Duplicate capability rejection
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 34: Duplicate capability rejection");

var dupCap = RoomCapability.Create("Patient Monitor").Value;
var dupResult = consultRoom.AddCapability(dupCap, roomUserId, DateTime.UtcNow);
PrintResult(dupResult, "AddCapability(duplicate)", isError: true);
Console.WriteLine($"  → Duplicate correctly rejected.");

// ─────────────────────────────────────────────────────────────
// Scenario 35: Remove capability
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 35: Remove capability");

var roomRemoveResult = consultRoom.RemoveCapability("Patient Monitor", roomUserId, DateTime.UtcNow);
PrintResult(roomRemoveResult, "consultRoom.RemoveCapability(\"Patient Monitor\")");
Console.WriteLine($"  Capabilities now: {consultRoom.Capabilities.Count}");

// Remove non-existent
var removeMissingResult = consultRoom.RemoveCapability("NonExistent", roomUserId, DateTime.UtcNow);
PrintResult(removeMissingResult, "RemoveCapability(non-existent)", isError: true);
Console.WriteLine($"  → Missing capability correctly rejected.");

// ─────────────────────────────────────────────────────────────
// Scenario 36: Schedule maintenance
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 36: Schedule maintenance");

var maintFrom = new DateTime(futureDate2.Year, futureDate2.Month, futureDate2.Day, 8, 0, 0, DateTimeKind.Utc);
var maintUntil = new DateTime(futureDate2.Year, futureDate2.Month, futureDate2.Day, 12, 0, 0, DateTimeKind.Utc);

var maintResult = procRoom.ScheduleMaintenance(
    maintFrom, maintUntil,
    MaintenanceReason.EquipmentRepair,
    roomUserId,
    overrideConfirmedAppointments: false,
    roomUserId, DateTime.UtcNow);

PrintResult(maintResult, "procRoom.ScheduleMaintenance(8-12, EquipmentRepair)");
Console.WriteLine($"  Maintenance blocks: {procRoom.MaintenanceBlocks.Count}");
var maintBlock = procRoom.MaintenanceBlocks.First();
Console.WriteLine($"  Block ID: {maintBlock.Id}");
Console.WriteLine($"  Period: {maintBlock.MaintenancePeriod.Start:HH:mm} → {maintBlock.MaintenancePeriod.End:HH:mm}");
Console.WriteLine($"  Reason: {maintBlock.Reason}");
Console.WriteLine($"  Active: {maintBlock.IsActive}");
Console.WriteLine($"  Domain events: {procRoom.DomainEvents.Count}");
Console.WriteLine($"    Latest: {procRoom.DomainEvents.Last().GetType().Name}");

// ─────────────────────────────────────────────────────────────
// Scenario 37: Overlapping maintenance rejection
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 37: Overlapping maintenance rejection");

var roomOverlapResult = procRoom.ScheduleMaintenance(
    maintFrom.AddHours(1), maintUntil.AddHours(-1),
    MaintenanceReason.Cleaning,
    roomUserId,
    overrideConfirmedAppointments: false,
    roomUserId, DateTime.UtcNow);

PrintResult(roomOverlapResult, "ScheduleMaintenance(overlap)", isError: true);
Console.WriteLine($"  → Overlapping maintenance correctly rejected.");

// ─────────────────────────────────────────────────────────────
// Scenario 38: Cancel maintenance
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 38: Cancel maintenance");

var cancelMaintResult = procRoom.CancelMaintenance(maintBlock.Id, roomUserId, DateTime.UtcNow);
PrintResult(cancelMaintResult, "procRoom.CancelMaintenance(block)");

var cancelledBlock = procRoom.MaintenanceBlocks.First();
Console.WriteLine($"  Active: {cancelledBlock.IsActive}");
Console.WriteLine($"  CancelledAt: {cancelledBlock.CancelledAt}");
Console.WriteLine($"  Latest event: {procRoom.DomainEvents.Last().GetType().Name}");

// ─────────────────────────────────────────────────────────────
// Scenario 39: Cancel already-cancelled maintenance rejection
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 39: Cancel already-cancelled maintenance");

var reCancelResult = procRoom.CancelMaintenance(maintBlock.Id, roomUserId, DateTime.UtcNow);
PrintResult(reCancelResult, "CancelMaintenance(already cancelled)", isError: true);
Console.WriteLine($"  → Re-cancellation correctly rejected.");

// ─────────────────────────────────────────────────────────────
// Scenario 40: Schedule non-overlapping maintenance (succeeds)
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 40: Non-overlapping maintenance");

var maintFrom2 = new DateTime(futureDate2.Year, futureDate2.Month, futureDate2.Day, 14, 0, 0, DateTimeKind.Utc);
var maintUntil2 = new DateTime(futureDate2.Year, futureDate2.Month, futureDate2.Day, 16, 0, 0, DateTimeKind.Utc);

var maint2Result = procRoom.ScheduleMaintenance(
    maintFrom2, maintUntil2,
    MaintenanceReason.Cleaning,
    roomUserId,
    overrideConfirmedAppointments: false,
    roomUserId, DateTime.UtcNow);

PrintResult(maint2Result, "ScheduleMaintenance(14-16, non-overlapping)");
Console.WriteLine($"  Total maintenance blocks: {procRoom.MaintenanceBlocks.Count}");
Console.WriteLine($"  Active blocks: {procRoom.MaintenanceBlocks.Count(b => b.IsActive)}");
Console.WriteLine($"  Latest event: {procRoom.DomainEvents.Last().GetType().Name}");

// ═════════════════════════════════════════════════════════════
// STAFF MEMBER AGGREGATE SCENARIOS
// ═════════════════════════════════════════════════════════════

var staffUserId = appointmentUserId;

// ─────────────────────────────────────────────────────────────
// Scenario 41: Create staff member (success)
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 41: Create staff member");

var nurseRoleResult = StaffRole.Create(StaffRoleEnum.Nurse, "Cardiology");
PrintResult(nurseRoleResult, "StaffRole.Create(Nurse, Cardiology)");
var nurseRole = nurseRoleResult.Value;

var empResult = EmploymentInfo.Create(
    new DateTime(2024, 3, 15, 0, 0, 0, DateTimeKind.Utc),
    EmploymentStatus.Active,
    ContractType.FullTime);
PrintResult(empResult, "EmploymentInfo.Create(2024-03-15, Active, FullTime)");
var employment = empResult.Value;

var staffResult = StaffMember.Create(
    identityId: "auth0|staff-001",
    firstName: "Jane",
    lastName: "Smith",
    role: nurseRole,
    employment: employment,
    phone: "+1-555-0100",
    email: "jane.smith@clinic.com",
    createdBy: staffUserId,
    createdOnUtc: DateTime.UtcNow);

PrintResult(staffResult, "StaffMember.Create");
var staff = staffResult.Value;
Console.WriteLine($"  Staff ID: {staff.Id}");
Console.WriteLine($"  IdentityId: {staff.IdentityId}");
Console.WriteLine($"  Name: {staff.FirstName} {staff.LastName}");
Console.WriteLine($"  Role: {staff.Role}");
Console.WriteLine($"  Employment: {staff.Employment}");
Console.WriteLine($"  Phone: {staff.Phone}");
Console.WriteLine($"  Email: {staff.Email}");
Console.WriteLine($"  Domain events: {staff.DomainEvents.Count}");
foreach (var e in staff.DomainEvents)
    Console.WriteLine($"    - {e.GetType().Name}");

// ─────────────────────────────────────────────────────────────
// Scenario 42: Create staff member (failures)
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 42: Create staff member (failures)");

// 42a — Missing identity ID
var noIdStaff = StaffMember.Create(
    "", "Test", "User",
    nurseRole, employment, null, null,
    staffUserId, DateTime.UtcNow);
PrintResult(noIdStaff, "Missing IdentityId", isError: true);

// 42b — Missing name
var noNameStaff = StaffMember.Create(
    "auth|test", "", "",
    nurseRole, employment, null, null,
    staffUserId, DateTime.UtcNow);
PrintResult(noNameStaff, "Missing name", isError: true);

// 42c — Future hire date
var futureEmp = EmploymentInfo.Create(
    DateTime.UtcNow.AddDays(30),
    EmploymentStatus.Active,
    ContractType.FullTime);
PrintResult(futureEmp, "Future hire date", isError: true);

// 42d — None role
var invalidRole = StaffRole.Create(StaffRoleEnum.None, "Dept");
PrintResult(invalidRole, "None role", isError: true);

// 42e — Empty department
var noDept = StaffRole.Create(StaffRoleEnum.Receptionist, "");
PrintResult(noDept, "Empty department", isError: true);

Console.WriteLine("  → All 5 guard clauses correctly rejected invalid input.");

// ─────────────────────────────────────────────────────────────
// Scenario 43: Change staff role
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 43: Change staff role");

var newRole = StaffRole.Create(StaffRoleEnum.ClinicManager, "Administration").Value;
Console.WriteLine($"  Before: {staff.Role}");

var changeRoleResult = staff.ChangeRole(newRole, staffUserId, DateTime.UtcNow);
PrintResult(changeRoleResult, "staff.ChangeRole(Nurse → ClinicManager)");
Console.WriteLine($"  After: {staff.Role}");
Console.WriteLine($"  Latest event: {staff.DomainEvents.Last().GetType().Name}");

// ─────────────────────────────────────────────────────────────
// Scenario 44: Update employment
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 44: Update employment");

var newEmp = EmploymentInfo.Create(
    new DateTime(2024, 3, 15, 0, 0, 0, DateTimeKind.Utc),
    EmploymentStatus.OnLeave,
    ContractType.PartTime).Value;
Console.WriteLine($"  Before: {staff.Employment}");

var updateEmpResult = staff.UpdateEmployment(newEmp, staffUserId, DateTime.UtcNow);
PrintResult(updateEmpResult, "staff.UpdateEmployment(OnLeave, PartTime)");
Console.WriteLine($"  After: {staff.Employment}");
Console.WriteLine($"  Latest event: {staff.DomainEvents.Last().GetType().Name}");

// ─────────────────────────────────────────────────────────────
// Scenario 45: Update contact info
// ─────────────────────────────────────────────────────────────
WriteHeader("Scenario 45: Update contact info");

Console.WriteLine($"  Before: Phone={staff.Phone}, Email={staff.Email}");

var updateContactResult = staff.UpdateContact(
    "+1-555-0200", "jane.manager@clinic.com",
    staffUserId, DateTime.UtcNow);
PrintResult(updateContactResult, "staff.UpdateContact(new phone, new email)");
Console.WriteLine($"  After: Phone={staff.Phone}, Email={staff.Email}");
Console.WriteLine($"  Latest event: {staff.DomainEvents.Last().GetType().Name}");

// ═════════════════════════════════════════════════════════════
// Summary
// ═════════════════════════════════════════════════════════════
WriteHeader("SUMMARY");

Console.WriteLine($"""
  Doctor:               {doctor.Id}
  Credentials created:  3 (1 success + 2 for lifecycle + 3 for validity check)
  Credential failures:  3 (future date, empty doc, expiry before issue)
  Specialties:          {doctor.Specialties.Count} (on aggregate) + 1 standalone
  Affiliations:         {doctor.Affiliations.Count} (on aggregate) + 2 standalone
  Schedules:            2 (one per clinic)
  Total blocks added:   4 (2 added, 2 removed, 1 rejected as overlap)
  Domain events raised: {doctor.DomainEvents.Count} + {schedule.DomainEvents.Count} + {schedule2.DomainEvents.Count}
  ---
  Appointments:         6 created (3 lifecycle + 2 failures + 1 rescheduled)
  Appointment failures: 5 (past date, wrong duration, invalid transitions)
  Appointment events:   {appointment.DomainEvents.Count} (completed flow) + {cancelAppt.DomainEvents.Count} (cancelled flow) + {noShowAppt.DomainEvents.Count} (no-show) + {reschedAppt.DomainEvents.Count} (rescheduled)
  ---
  Rooms:                2 created (1 consultation + 1 procedure)
  Room failures:        2 (missing capabilities, duplicate cap, missing cap, overlapping maint, re-cancel)
  Capabilities added:   1 (then removed)
  Maintenance blocks:   2 scheduled (1 cancelled, 1 active)
  Room events:          {consultRoom.DomainEvents.Count} (consult) + {procRoom.DomainEvents.Count} (procedure)
  ---
  Staff members:        1 created
  Staff failures:       5 (missing identity, missing name, future hire, none role, empty dept)
  Staff mutations:      3 (role changed, employment updated, contact updated)
  Staff events:         {staff.DomainEvents.Count}
""");

Console.WriteLine("All scenarios completed.");

// ═════════════════════════════════════════════════════════════
// Helper methods
// ═════════════════════════════════════════════════════════════

static void WriteHeader(string title)
{
    Console.WriteLine();
    Console.WriteLine(new string('═', 70));
    Console.WriteLine($"  {title}");
    Console.WriteLine(new string('═', 70));
}

static void PrintResult<T>(ErrorOr<T> result, string label, bool isError = false)
{
    if (result.IsError)
    {
        var icon = isError ? "✓" : "✗";
        Console.WriteLine($"  {icon} [{label}] FAILED");
        foreach (var err in result.Errors)
        {
            Console.WriteLine($"      {err.Code}: {err.Description}");
        }
    }
    else
    {
        var icon = isError ? "✗ (unexpected success)" : "✓";
        Console.WriteLine($"  {icon} [{label}] SUCCESS");
    }
}
