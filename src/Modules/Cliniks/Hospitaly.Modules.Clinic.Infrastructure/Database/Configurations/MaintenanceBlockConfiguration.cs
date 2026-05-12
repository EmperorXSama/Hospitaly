using Hospitaly.Modules.Clinic.Domain.Room;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Configurations;

public class MaintenanceBlockConfiguration : IEntityTypeConfiguration<MaintenanceBlock>
{
    public void Configure(EntityTypeBuilder<MaintenanceBlock> builder)
    {
        builder.ToTable("MaintenanceBlocks");

        builder.HasKey(mb => mb.Id);

        builder.Property(mb => mb.RoomId).IsRequired();
        builder.Property(mb => mb.Reason).HasConversion<string>().HasMaxLength(30);
        builder.Property(mb => mb.ScheduledBy).IsRequired();
        builder.Property(mb => mb.CancelledAt).HasColumnType("timestamp with time zone");

        builder.OwnsOne(mb => mb.MaintenancePeriod, period =>
        {
            period.Property(p => p.Start).HasColumnType("timestamp with time zone").IsRequired();
            period.Property(p => p.End).HasColumnType("timestamp with time zone");
        });

        builder.HasIndex(mb => mb.RoomId);
    }
}
