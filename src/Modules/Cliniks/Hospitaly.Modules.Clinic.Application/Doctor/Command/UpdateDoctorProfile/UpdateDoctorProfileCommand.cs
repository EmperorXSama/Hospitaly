using Hospitaly.Common.Application.Abstraction.Messaging;
using ErrorOr;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.UpdateDoctorProfile;

public sealed record UpdateDoctorProfileCommand(
    Guid DoctorId,
    string? Title,
    string? Bio,
    string? AvatarUrl,
    Guid UserId) : ICommand<Success>;
