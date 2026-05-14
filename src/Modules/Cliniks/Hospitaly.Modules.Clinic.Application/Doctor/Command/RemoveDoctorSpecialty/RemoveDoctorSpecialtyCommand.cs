using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.RemoveDoctorSpecialty;

public sealed record RemoveDoctorSpecialtyCommand(
    Guid DoctorId,
    Guid SpecialtyId,
    Guid UserId) : ICommand<Success>;
