namespace Hospitaly.Modules.Clinic.Application.Clinic.Queries.GetClinicOwnerships;

public sealed record ClinicOwnershipResponse
{
    public Guid Id { get; init; }
    public Guid OwnerId { get; init; }
    public string OwnerShipType { get; init; }
    public decimal SharePercentage { get; init; }
    public DateTimeOffset EffectiveStart { get; init; }
    public DateTimeOffset? EffectiveEnd { get; init; }
    public string Status { get; init; }
    public OwnerInfo? Owner { get; init; }
};

public sealed record OwnerInfo
{
    public Guid UserId { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Sex { get; init; } = string.Empty;
    public DateOnly DateOfBirth { get; init; }
}
