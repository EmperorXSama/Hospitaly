using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorById;

public sealed record GetDoctorByIdQuery(Guid DoctorId) : IQuery<DoctorDetailResponse>;
