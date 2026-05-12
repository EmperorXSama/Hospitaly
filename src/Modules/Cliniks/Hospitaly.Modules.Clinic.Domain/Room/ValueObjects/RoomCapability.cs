using ErrorOr;

namespace Hospitaly.Modules.Clinic.Domain.Room.ValueObjects;

public sealed record RoomCapability
{
    public string Name { get; }

    private RoomCapability()
    {
    }

    private RoomCapability(string name)
    {
        Name = name;
    }

    public static ErrorOr<RoomCapability> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Error.Validation(
                "RoomCapability.NameRequired",
                "Capability name is required.");
        }

        return new RoomCapability(name);
    }

    public override string ToString() => Name;
}
