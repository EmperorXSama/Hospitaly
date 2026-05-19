using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicOwnerships;

public sealed record GetClinicOwnershipsQuery(Guid ClinicId) : IQuery<List<ClinicOwnershipResponse>>;
