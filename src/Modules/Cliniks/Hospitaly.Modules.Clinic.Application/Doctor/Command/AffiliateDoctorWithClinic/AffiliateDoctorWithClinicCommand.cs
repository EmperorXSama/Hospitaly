using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.AffiliateDoctorWithClinic;

public sealed record AffiliationPrivilegeItem(
    string PrivilegeType,
    DateTime GrantedAt);

public sealed record AffiliateDoctorWithClinicCommand(
    Guid DoctorId,
    Guid ClinicId,
    DateTime JoinedDate,
    Guid? DepartmentId,
    List<AffiliationPrivilegeItem>? GrantedPrivileges,
    Guid UserId) : ICommand<Guid>;
