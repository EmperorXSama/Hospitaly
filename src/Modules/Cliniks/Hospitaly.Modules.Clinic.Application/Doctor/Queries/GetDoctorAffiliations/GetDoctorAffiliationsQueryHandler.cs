using Dapper;
using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application.Data;

namespace Hospitaly.Modules.Clinic.Application.Doctor.Queries.GetDoctorAffiliations;

internal sealed class GetDoctorAffiliationsQueryHandler(IDbConnectionFactory factory)
    : IQueryHandler<GetDoctorAffiliationsQuery, List<DoctorAffiliationResponse>>
{
    public async Task<ErrorOr<List<DoctorAffiliationResponse>>> Handle(
        GetDoctorAffiliationsQuery request,
        CancellationToken cancellationToken)
    {
        await using var connection = await factory.OpenConnectionAsync();

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
            ORDER BY ca."JoinedDate" DESC
            """;

        var affiliations = (await connection.QueryAsync<AffiliationRow>(affiliationsSql, new { request.DoctorId })).ToList();

        var affiliationIds = affiliations.Select(a => a.Id).ToList();
        var privilegeLookup = new Dictionary<Guid, List<PrivilegeResponse>>();

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
                    .Select(p => new PrivilegeResponse(p.Type, p.GrantedAt, p.GrantedBy))
                    .ToList();
            }
        }

        return affiliations.Select(a => new DoctorAffiliationResponse(
            a.Id, a.ClinicId, a.ClinicName, a.Status, a.JoinedDate, a.TerminatedDate,
            a.DepartmentId, privilegeLookup.GetValueOrDefault(a.Id, []))).ToList();
    }

    private sealed record AffiliationRow(
        Guid Id, Guid ClinicId, string? ClinicName, string Status,
        DateTime JoinedDate, DateTime? TerminatedDate, Guid? DepartmentId);

    private sealed record PrivilegeRow(
        Guid ClinicAffiliationId, string Type, DateTime GrantedAt, Guid GrantedBy);
}
