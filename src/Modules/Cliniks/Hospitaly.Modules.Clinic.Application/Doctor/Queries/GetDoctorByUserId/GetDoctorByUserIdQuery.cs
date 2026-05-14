using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorById;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorByUserId;

public sealed record GetDoctorByUserIdQuery(Guid UserId) : IQuery<DoctorDetailResponse>;
