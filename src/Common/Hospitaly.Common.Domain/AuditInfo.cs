namespace Hospitaly.Common.Domain;

public sealed class AuditInfo
{
    public Guid CreatedBy { get; private set; }
    public DateTimeOffset CreatedOnUtc { get; private set; }
    public Guid? UpdatedBy { get; private set; }
    public DateTimeOffset? UpdatedOnUtc { get; private set; }

    private AuditInfo() { } // EF Core

    public AuditInfo(Guid createdBy, DateTimeOffset createdOnUtc)
    {
        CreatedBy = createdBy;
        CreatedOnUtc = createdOnUtc;
    }

    // Mutates in place — EF Core keeps the same reference
    public void ApplyUpdate(Guid updatedBy, DateTimeOffset updatedOnUtc)
    {
        UpdatedBy = updatedBy;
        UpdatedOnUtc = updatedOnUtc;
    }
}