using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.SuspendClinicAffiliation;

public sealed record SuspendClinicAffiliationCommand(
    Guid DoctorId,
    Guid ClinicId,
    Guid UserId) : ICommand<Success>;
