using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.CreateClinic;

public sealed record CreateClinicCommand(
    Guid UserId,
    string Name,
    string Description,
    string Street,
    string City,
    string? Region,
    string? PostalCode,
    string Country,
    string? Phone,
    string? Email) : ICommand<Guid>;
