using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicOperatingLicense;

public sealed record GetClinicOperatingLicenseQuery(Guid ClinicId) : IQuery<ClinicOperatingLicenseResponse>;
