namespace Hospitaly.Common.Domain;
public sealed record AuditInfo(
    Guid CreatedBy,
    DateTimeOffset CreatedOnUtc,
    Guid? UpdatedBy = null,
    DateTimeOffset? UpdatedOnUtc = null)
{
    public AuditInfo WithUpdate(Guid updatedBy, DateTimeOffset updatedOnUtc) =>
        this with { UpdatedBy = updatedBy, UpdatedOnUtc = updatedOnUtc };
}