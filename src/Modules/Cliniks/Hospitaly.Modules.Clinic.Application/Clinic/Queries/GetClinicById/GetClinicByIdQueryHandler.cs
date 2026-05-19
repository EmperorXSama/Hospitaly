using Dapper;
using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application.Data;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicById;

internal sealed class GetClinicByIdQueryHandler(IDbConnectionFactory factory)
    : IQueryHandler<GetClinicByIdQuery, ClinicDetailResponse>
{
    public async Task<ErrorOr<ClinicDetailResponse>> Handle(
        GetClinicByIdQuery request,
        CancellationToken cancellationToken)
    {
        await using var connection = await factory.OpenConnectionAsync();

        const string sql = """
            SELECT
                c."Id" AS Id,
                c."Info_Name" AS Name,
                c."Info_TradingName" AS TradingName,
                c."Info_Description" AS Description,
                c."Info_LogoUrl" AS LogoUrl,
                c."Address_Value_Street" AS Street,
                c."Address_Value_City" AS City,
                c."Address_Value_Region" AS Region,
                c."Address_Value_PostalCode" AS PostalCode,
                c."Address_Value_Country" AS Country,
                c."Address_Coordinates_Latitude" AS Latitude,
                c."Address_Coordinates_Longitude" AS Longitude,

                c."ContactInfo_PhoneNumber_Value" AS Phone,
                c."ContactInfo_Email_Value" AS Email,
                c."ContactInfo_Website" AS Website,

                ol."LicenseNumber" AS LicenseNumber,
                ol."IssuingAuthority" AS IssuingAuthority,
                ol."LicenseType" AS LicenseType,
                ol."ValidityPeriod_Value_Start" AS LicenseValidityStart,
                ol."ValidityPeriod_Value_End" AS LicenseValidityEnd,
                ol."AdministrativeStatus" AS LicenseAdministrativeStatus

            FROM clinics."Clinics" c
            LEFT JOIN clinics."OperatingLicenses" ol ON ol."ClinicId" = c."Id"
            WHERE c."Id" = @ClinicId
            """;

        var result = await connection.QueryFirstOrDefaultAsync<ClinicDetailResponse>(
            sql, new { request.ClinicId });

        if (result is null)
            return Error.NotFound(
                code: "GetClinicById.NotFound",
                description: $"Clinic with id {request.ClinicId} was not found.");

        return result;
    }
}
