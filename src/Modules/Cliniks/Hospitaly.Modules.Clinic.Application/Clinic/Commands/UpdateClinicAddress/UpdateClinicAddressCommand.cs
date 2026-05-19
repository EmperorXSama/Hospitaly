using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateClinicAddress;

public sealed record UpdateClinicAddressCommand(
    Guid ClinicId,
    string Street,
    string City,
    string? Region,
    string? PostalCode,
    string Country,
    double? Latitude,
    double? Longitude,
    Guid UserId) : ICommand<Success>;
