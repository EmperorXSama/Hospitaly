using ErrorOr;
using Hospitaly.Modules.Clinic.Domain.Doctor.Enums;

namespace Hospitaly.Modules.Clinic.Domain.Doctor.ValueObjects;

public sealed record Privilege
{
    public PrivilegeType Type { get; init; }
    public DateTime GrantedAt { get; init; }
    public Guid GrantedBy { get; init; }

    private Privilege()
    {
    }

    private Privilege(PrivilegeType type, DateTime grantedAt, Guid grantedBy)
    {
        Type = type;
        GrantedAt = grantedAt;
        GrantedBy = grantedBy;
    }

    public static ErrorOr<Privilege> Create(PrivilegeType type, DateTime grantedAt, Guid grantedBy)
    {
        var errors = new List<Error>();

        if (grantedAt > DateTime.UtcNow)
        {
            errors.Add(Error.Validation(
                code: "Privilege.GrantedAtInFuture",
                description: "Privilege grant date cannot be in the future.",
                metadata: new Dictionary<string, object> { ["grantedAt"] = grantedAt }));
        }

        if (grantedBy == Guid.Empty)
        {
            errors.Add(Error.Validation(
                code: "Privilege.InvalidGrantedBy",
                description: "GrantedBy identifier cannot be empty."));
        }

        if (errors.Any())
        {
            return errors;
        }

        return new Privilege(type, grantedAt, grantedBy);
    }
}
