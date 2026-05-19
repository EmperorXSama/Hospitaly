namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetMyClinics;

public sealed record ClinicListItemResponse(
    Guid ClinicId,
    string Name,
    string OwnerShipType,
    string Status);
