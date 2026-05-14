using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.ActivateClinicAffiliation;

public sealed record ActivateClinicAffiliationCommand(
    Guid DoctorId,
    Guid ClinicId,
    Guid UserId) : ICommand<Success>;
