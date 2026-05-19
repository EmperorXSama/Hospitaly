using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.SetPrimaryDoctorSpecialty;

public sealed record SetPrimaryDoctorSpecialtyCommand(
    Guid DoctorId,
    Guid SpecialtyId,
    Guid UserId) : ICommand<Success>;
