using Dapper;
using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application.Data;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorCredentials;

internal sealed class GetDoctorCredentialsQueryHandler(IDbConnectionFactory factory)
    : IQueryHandler<GetDoctorCredentialsQuery, List<DoctorCredentialResponse>>
{
    public async Task<ErrorOr<List<DoctorCredentialResponse>>> Handle(
        GetDoctorCredentialsQuery request,
        CancellationToken cancellationToken)
    {
        await using var connection = await factory.OpenConnectionAsync();

        var sql = """
            SELECT
                dc."Id",
                dc."CredentialType",
                dc."IssuingAuthority",
                dc."DocumentNumber",
                dc."Status",
                dc."VerifiedAt",
                dc."VerifiedBy",
                dc."ValidityPeriod_Value_Start" AS IssueDate,
                dc."ValidityPeriod_Value_End" AS ExpiryDate
            FROM clinics."DoctorCredentials" dc
            WHERE dc."DoctorId" = @DoctorId
            ORDER BY dc."ValidityPeriod_Value_Start" DESC
            """;

        var result = (await connection.QueryAsync<DoctorCredentialResponse>(sql, new { request.DoctorId })).ToList();
        return result;
    }
}
