using ErrorOr;

namespace Hospitaly.Modules.Users.Domain.Users;

public sealed class BloodType
{
    public static readonly BloodType APos = new("A+");
    public static readonly BloodType ANeg = new("A-");
    public static readonly BloodType BPos = new("B+");
    public static readonly BloodType BNeg = new("B-");
    public static readonly BloodType ABPos = new("AB+");
    public static readonly BloodType ABNeg = new("AB-");
    public static readonly BloodType OPos = new("O+");
    public static readonly BloodType ONeg = new("O-");

    public static readonly IReadOnlyCollection<BloodType> All =
    [
        APos, ANeg, BPos, BNeg, ABPos, ABNeg, OPos, ONeg
    ];

    public static ErrorOr<BloodType> FromName(string name)
    {
        var bloodType = All.FirstOrDefault(bt => bt.Name == name);
        return bloodType is not null
            ? bloodType
            : Error.Validation("BloodType.Invalid", $"'{name}' is not a valid blood type");
    }

    private BloodType(string name) => Name = name;
    private BloodType() { }
    public string Name { get; private set; }

    public override string ToString() => Name;
}
