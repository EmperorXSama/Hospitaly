using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicSpecialties;

public sealed record GetClinicSpecialtiesQuery(Guid ClinicId) : IQuery<List<ClinicSpecialtyResponse>>;
