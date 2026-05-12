

namespace Hospitaly.Common.Domain;

public abstract class Entity: IEquatable<Entity>
{
    public Guid Id { get; protected set; }
    public AuditInfo Audit { get; private set; }
    protected Entity(){}
    protected Entity(
        AuditInfo audit)
    {
        Id = audit.CreatedBy;
        Audit = audit;
    }
    protected void SetUpdated(Guid updatedBy, DateTimeOffset updatedOnUtc) =>
        Audit = Audit.WithUpdate(updatedBy, updatedOnUtc);
    public override bool Equals(object? obj)
    {
        return obj is Entity entity && Id.Equals(entity.Id);
    }

    public static bool operator ==(Entity left, Entity right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }

    public bool Equals(Entity? other)
    {
        return Equals((object?)other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

}