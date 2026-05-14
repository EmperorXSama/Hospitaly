using Dapper;
using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application.Data;
using Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorById;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorByUserId;

internal sealed class GetDoctorByUserIdQueryHandler(IDbConnectionFactory factory)
    : IQueryHandler<GetDoctorByUserIdQuery, DoctorDetailResponse>
{
    public async Task<ErrorOr<DoctorDetailResponse>> Handle(
        GetDoctorByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        await using var connection = await factory.OpenConnectionAsync();

        var doctorSql = """
            SELECT
                d."Id",
                d."Status",
                d."Title",
                d."Bio",
                d."AvatarUrl",
                d."CreatedBy",
                d."CreatedOnUtc"
            FROM clinics."Doctors" d
            WHERE d."CreatedBy" = @UserId
            """;

        var doctor = await connection.QueryFirstOrDefaultAsync<DoctorRow>(doctorSql, new { request.UserId });
        if (doctor is null)
            return Error.NotFound("GetDoctorByUserId.NotFound", $"Doctor for user {request.UserId} was not found.");

        var credentialsSql = """
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
            """;

        var credentials = (await connection.QueryAsync<DoctorCredentialItem>(credentialsSql, new { doctor.Id })).ToList();

        var specialtiesSql = """
            SELECT
                ds."SpecialtyId",
                s."Name" AS SpecialtyName,
                ds."IsPrimary",
                ds."CertificationNumber",
                ds."CertifiedAt"
            FROM clinics."DoctorSpecialties" ds
            JOIN clinics."Specialties" s ON s."Id" = ds."SpecialtyId"
            WHERE ds."DoctorId" = @DoctorId
            """;

        var specialties = (await connection.QueryAsync<DoctorSpecialtyItem>(specialtiesSql, new { doctor.Id })).ToList();

        var affiliationsSql = """
            SELECT
                ca."Id",
                ca."ClinicId",
                c."Info_Name" AS ClinicName,
                ca."Status",
                ca."JoinedDate",
                ca."TerminatedDate",
                ca."DepartmentId"
            FROM clinics."ClinicAffiliations" ca
            LEFT JOIN clinics."Clinics" c ON c."Id" = ca."ClinicId"
            WHERE ca."DoctorId" = @DoctorId
            """;

        var affiliations = (await connection.QueryAsync<AffiliationRow>(affiliationsSql, new { doctor.Id })).ToList();

        var affiliationIds = affiliations.Select(a => a.Id).ToList();
        var privilegeLookup = new Dictionary<Guid, List<PrivilegeItem>>();

        if (affiliationIds.Count != 0)
        {
            var privilegesSql = """
                SELECT "ClinicAffiliationId", "Type", "GrantedAt", "GrantedBy"
                FROM clinics."ClinicAffiliationPrivileges"
                WHERE "ClinicAffiliationId" = ANY(@Ids)
                """;

            var privilegeRows = await connection.QueryAsync<PrivilegeRow>(privilegesSql, new { Ids = affiliationIds });

            foreach (var group in privilegeRows.GroupBy(p => p.ClinicAffiliationId))
            {
                privilegeLookup[group.Key] = group
                    .Select(p => new PrivilegeItem(p.Type, p.GrantedAt, p.GrantedBy))
                    .ToList();
            }
        }

        var affiliationItems = affiliations.Select(a => new DoctorAffiliationItem(
            a.Id, a.ClinicId, a.ClinicName, a.Status, a.JoinedDate, a.TerminatedDate,
            a.DepartmentId, privilegeLookup.GetValueOrDefault(a.Id, []))).ToList();

        return new DoctorDetailResponse(
            doctor.Id, doctor.Status, doctor.Title, doctor.Bio, doctor.AvatarUrl,
            doctor.CreatedBy, doctor.CreatedOnUtc,
            credentials, specialties, affiliationItems);
    }

    private sealed record DoctorRow(
        Guid Id, string Status, string? Title, string? Bio, string? AvatarUrl,
        Guid CreatedBy, DateTime CreatedOnUtc);

    private sealed record AffiliationRow(
        Guid Id, Guid ClinicId, string? ClinicName, string Status,
        DateTime JoinedDate, DateTime? TerminatedDate, Guid? DepartmentId);

    private sealed record PrivilegeRow(
        Guid ClinicAffiliationId, string Type, DateTime GrantedAt, Guid GrantedBy);
}
