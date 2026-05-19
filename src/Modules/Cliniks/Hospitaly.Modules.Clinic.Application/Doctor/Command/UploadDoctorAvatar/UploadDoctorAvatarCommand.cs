using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.UploadDoctorAvatar;

public sealed record UploadDoctorAvatarCommand(
    Guid DoctorId,
    string AvatarUrl,
    Guid UserId) : ICommand<Success>;
