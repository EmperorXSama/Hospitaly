using RoomEntity = Hospitaly.Modules.Clinic.Domain.Room.Room;
using Hospitaly.Modules.Clinic.Domain.Room;
using Hospitaly.Modules.Clinic.Domain.Room.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<RoomEntity>
{
    public void Configure(EntityTypeBuilder<RoomEntity> builder)
    {
        builder.ToTable("Rooms");

        builder.HasKey(r => r.Id);

        builder.Ignore(r => r.Capabilities);
        builder.Ignore(r => r.MaintenanceBlocks);

        builder.Property(r => r.Name).HasMaxLength(200).IsRequired();

        builder.OwnsOne(r => r.RoomType, roomType =>
        {
            roomType.Property(rt => rt.Type)
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();
        });

        builder.OwnsMany<RoomCapability>("_capabilities", capabilities =>
        {
            capabilities.ToTable("RoomCapabilities");
            capabilities.WithOwner().HasForeignKey("RoomId");

            capabilities.Property(c => c.Name).HasMaxLength(200).IsRequired();
        });

        builder.HasMany<MaintenanceBlock>("_maintenanceBlocks")
            .WithOne()
            .HasForeignKey(mb => mb.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
