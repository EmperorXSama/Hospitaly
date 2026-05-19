using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetMyClinics;

public sealed record GetMyClinicsQuery(Guid UserId) : IQuery<List<ClinicListItemResponse>>;
