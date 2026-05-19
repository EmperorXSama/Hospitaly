using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicById;

public sealed record GetClinicByIdQuery(Guid ClinicId) : IQuery<ClinicDetailResponse>;
