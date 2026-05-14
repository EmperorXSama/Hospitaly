using Dapper;
using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application.Data;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicOperatingLicense;

internal sealed class GetClinicOperatingLicenseQueryHandler(IDbConnectionFactory factory)
    : IQueryHandler<GetClinicOperatingLicenseQuery, ClinicOperatingLicenseResponse>
{
    public async Task<ErrorOr<ClinicOperatingLicenseResponse>> Handle(
        GetClinicOperatingLicenseQuery request,
        CancellationToken cancellationToken)
    {
        await using var connection = await factory.OpenConnectionAsync();

        const string sql = """
            SELECT
                ol."Id" AS Id,
                ol."LicenseNumber" AS LicenseNumber,
                ol."IssuingAuthority" AS IssuingAuthority,
                ol."LicenseType" AS LicenseType,
                ol."ValidityPeriod_Value_Start" AS ValidityStart,
                ol."ValidityPeriod_Value_End" AS ValidityEnd,
                ol."AdministrativeStatus" AS AdministrativeStatus
            FROM clinics."OperatingLicenses" ol
            WHERE ol."ClinicId" = @ClinicId
            """;

        var result = await connection.QueryFirstOrDefaultAsync<ClinicOperatingLicenseResponse>(
            sql, new { request.ClinicId });

        if (result is null)
            return Error.NotFound(
                code: "GetClinicOperatingLicense.NotFound",
                description: $"Operating license for clinic {request.ClinicId} was not found.");

        return result;
    }
}
