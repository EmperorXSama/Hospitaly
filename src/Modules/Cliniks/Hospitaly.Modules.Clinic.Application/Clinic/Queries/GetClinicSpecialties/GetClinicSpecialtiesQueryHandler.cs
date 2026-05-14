using Dapper;
using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application.Data;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicSpecialties;

internal sealed class GetClinicSpecialtiesQueryHandler(IDbConnectionFactory factory)
    : IQueryHandler<GetClinicSpecialtiesQuery, List<ClinicSpecialtyResponse>>
{
    public async Task<ErrorOr<List<ClinicSpecialtyResponse>>> Handle(
        GetClinicSpecialtiesQuery request,
        CancellationToken cancellationToken)
    {
        await using var connection = await factory.OpenConnectionAsync();

        const string sql = """
            SELECT
                cs."SpecialtyId" AS SpecialtyId,
                s."Name" AS SpecialtyName,
                cs."IsActive" AS IsActive,
                cs."ConsultationFee" AS ConsultationFee
            FROM clinics."ClinicSpecialties" cs
            INNER JOIN clinics."Specialties" s ON s."Id" = cs."SpecialtyId"
            WHERE cs."ClinicId" = @ClinicId
            ORDER BY s."Name"
            """;

        var result = (await connection.QueryAsync<ClinicSpecialtyResponse>(sql, new { request.ClinicId })).ToList();
        return result;
    }
}
