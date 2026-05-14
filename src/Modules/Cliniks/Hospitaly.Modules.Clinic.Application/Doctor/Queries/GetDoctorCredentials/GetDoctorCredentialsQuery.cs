using Hospitaly.Common.Application.Abstraction.Messaging;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorCredentials;

public sealed record GetDoctorCredentialsQuery(Guid DoctorId) : IQuery<List<DoctorCredentialResponse>>;
