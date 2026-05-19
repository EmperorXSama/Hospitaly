using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicOperatingHours;

public record ClinicOperatingHoursQuery(Guid ClinicId):IQuery<ClinicOperatingHoursResponse>;