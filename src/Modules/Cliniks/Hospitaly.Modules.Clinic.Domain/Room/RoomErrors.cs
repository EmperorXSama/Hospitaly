using ErrorOr;

namespace Hospitaly.Modules.Clinic.Domain.Room;

public static class RoomErrors
{
    public static Error NameRequired() =>
        Error.Validation("Room.NameRequired", "Room name is required.");

    public static Error RoomTypeRequired() =>
        Error.Validation("Room.RoomTypeRequired", "Room type must be a valid, non-empty type.");

    public static Error NonConsultationRoomRequiresCapabilities() =>
        Error.Validation(
            "Room.NonConsultationRoomRequiresCapabilities",
            "Non-consultation rooms must have at least one capability defined.");

    public static Error CapabilityNameRequired() =>
        Error.Validation("Room.CapabilityNameRequired", "Capability name is required.");

    public static Error DuplicateCapability(string name) =>
        Error.Conflict(
            "Room.DuplicateCapability",
            $"The room already has the capability '{name}'.");

    public static Error CapabilityNotFound(string name) =>
        Error.NotFound(
            "Room.CapabilityNotFound",
            $"The capability '{name}' was not found on this room.");

    public static Error OverlappingMaintenance() =>
        Error.Conflict(
            "Room.OverlappingMaintenance",
            "The maintenance period overlaps with an existing active maintenance block.");

    public static Error MaintenanceNotFound(Guid blockId) =>
        Error.NotFound(
            "Room.MaintenanceNotFound",
            $"The maintenance block with identifier {blockId} was not found.");

    public static Error MaintenanceAlreadyCancelled() =>
        Error.Conflict(
            "Room.MaintenanceAlreadyCancelled",
            "The maintenance block has already been cancelled.");

    public static Error ExtendMustBeLater() =>
        Error.Validation(
            "Room.ExtendMustBeLater",
            "The new end time must be later than the current end time.");

    public static Error RoomNotFound(Guid roomId) =>
        Error.NotFound(
            "Room.NotFound",
            $"The room with identifier {roomId} was not found.");
}
