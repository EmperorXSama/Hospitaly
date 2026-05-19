using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorAffiliations;

public sealed record GetDoctorAffiliationsQuery(Guid DoctorId) : IQuery<List<DoctorAffiliationResponse>>;
