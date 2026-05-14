using Dapper;
using ErrorOr;
using Hospitaly.Common.Application.Abstraction.Messaging;
using Hospitaly.Common.Application.Data;

namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicOperatingHours;

public class ClinicOperatingHoursQueryHandler(IDbConnectionFactory  factory):
    IQueryHandler<ClinicOperatingHoursQuery, ClinicOperatingHoursResponse>
{
    public async Task<ErrorOr<ClinicOperatingHoursResponse>> Handle(ClinicOperatingHoursQuery request, CancellationToken cancellationToken)
    {
        await using var connection = await factory.OpenConnectionAsync();

        const string sql = $"""
                            SELECT 
                                "ClinicId",
                                "Day",
                            
                                -- Operating Hours
                                "HoursActive",
                                "OpenTime",
                                "CloseTime",
                            
                                -- Resting Time
                                "RestingTimeActive",
                                "RestingStartTime",
                                "RestingEndTime"
                            
                            FROM clinics."ClinicOperatingHours"
                            WHERE "ClinicId" = @ClinicId;
                            """;
        var rows = await connection.QueryAsync<OperatingHoursDto>(sql, new { request.ClinicId });
        var response = new ClinicOperatingHoursResponse(
            ClinicId: request.ClinicId,
            OperatingHours: rows.ToList() 
        );
        return response;
    }
}