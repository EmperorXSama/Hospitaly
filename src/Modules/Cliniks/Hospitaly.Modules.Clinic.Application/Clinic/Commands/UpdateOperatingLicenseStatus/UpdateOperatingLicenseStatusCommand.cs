using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Clinic.Domain.Clinic.Enum;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Commands.UpdateOperatingLicenseStatus;

public sealed record UpdateOperatingLicenseStatusCommand(
    Guid ClinicId,
    LicenceAdministrativeStatus AdministrativeStatus,
    Guid UserId) : ICommand<Success>;
