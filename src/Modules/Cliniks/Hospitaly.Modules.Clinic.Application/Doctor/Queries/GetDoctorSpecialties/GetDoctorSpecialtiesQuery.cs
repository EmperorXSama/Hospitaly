using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorSpecialties;

public sealed record GetDoctorSpecialtiesQuery(Guid DoctorId) : IQuery<List<DoctorSpecialtyResponse>>;
