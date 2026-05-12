using ErrorOr;
using Hospitaly.Modules.Clinic.Domain.Room.Enums;

namespace Hospitaly.Modules.Clinic.Domain.Room.ValueObjects;

public sealed record RoomType
{
    public RoomCategory Type { get; }

    private RoomType()
    {
    }

    private RoomType(RoomCategory type)
    {
        Type = type;
    }

    public static ErrorOr<RoomType> Create(RoomCategory type)
    {
        if (type == RoomCategory.None)
        {
            return Error.Validation(
                "RoomType.Invalid",
                "Room type cannot be None.");
        }

        return new RoomType(type);
    }

    public override string ToString() => Type.ToString();
}
