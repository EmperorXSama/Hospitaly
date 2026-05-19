using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Clinic.Domain.Clinic.Enum;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.ReplaceOperatingLicense;

public sealed record ReplaceOperatingLicenseCommand(
    Guid ClinicId,
    string LicenseNumber,
    string IssuingAuthority,
    LicenseType LicenseType,
    DateTimeOffset ValidityStart,
    DateTimeOffset? ValidityEnd,
    LicenceAdministrativeStatus AdministrativeStatus,
    Guid UserId) : ICommand<Success>;
