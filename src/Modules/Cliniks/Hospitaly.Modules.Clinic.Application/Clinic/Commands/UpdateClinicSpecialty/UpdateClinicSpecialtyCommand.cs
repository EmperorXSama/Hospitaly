using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateClinicSpecialty;

public sealed record UpdateClinicSpecialtyCommand(
    Guid ClinicId,
    Guid SpecialtyId,
    bool IsActive,
    decimal? ConsultationFee,
    Guid UserId) : ICommand<Success>;
