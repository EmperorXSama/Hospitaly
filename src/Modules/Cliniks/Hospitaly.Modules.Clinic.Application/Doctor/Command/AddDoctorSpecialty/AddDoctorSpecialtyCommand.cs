using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Command.AddDoctorSpecialty;

public sealed record AddDoctorSpecialtyItem(
    Guid SpecialtyId,
    bool IsPrimary,
    string CertificationNumber,
    DateTime CertifiedAt);

public sealed record AddDoctorSpecialtyCommand(
    Guid DoctorId,
    List<AddDoctorSpecialtyItem> Specialties,
    Guid UserId) : ICommand<Success>;
