using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.AddClinicSpecialty;

public sealed record AddClinicSpecialtyCommand(
    Guid ClinicId,
    Guid SpecialtyId,
    bool IsActive,
    decimal? ConsultationFee,
    Guid UserId) : ICommand<Success>;
