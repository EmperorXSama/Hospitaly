using Dapper;
using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application.Data;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorSpecialties;

internal sealed class GetDoctorSpecialtiesQueryHandler(IDbConnectionFactory factory)
    : IQueryHandler<GetDoctorSpecialtiesQuery, List<DoctorSpecialtyResponse>>
{
    public async Task<ErrorOr<List<DoctorSpecialtyResponse>>> Handle(
        GetDoctorSpecialtiesQuery request,
        CancellationToken cancellationToken)
    {
        await using var connection = await factory.OpenConnectionAsync();

        var sql = """
            SELECT
                ds."SpecialtyId",
                s."Name" AS SpecialtyName,
                ds."IsPrimary",
                ds."CertificationNumber",
                ds."CertifiedAt"
            FROM clinics."DoctorSpecialties" ds
            JOIN clinics."Specialties" s ON s."Id" = ds."SpecialtyId"
            WHERE ds."DoctorId" = @DoctorId
            ORDER BY ds."IsPrimary" DESC, s."Name"
            """;

        var result = (await connection.QueryAsync<DoctorSpecialtyResponse>(sql, new { request.DoctorId })).ToList();
        return result;
    }
}
