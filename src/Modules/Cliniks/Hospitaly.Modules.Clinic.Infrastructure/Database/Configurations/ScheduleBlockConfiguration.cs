using Hospitaly.Modules.Clinic.Domain.DoctorSchedule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Configurations;

public class ScheduleBlockConfiguration : IEntityTypeConfiguration<ScheduleBlock>
{
    public void Configure(EntityTypeBuilder<ScheduleBlock> builder)
    {
        builder.ToTable("ScheduleBlocks");

        builder.HasKey(sb => sb.Id);

        builder.Property(sb => sb.DoctorScheduleId).IsRequired();
        builder.Property(sb => sb.DayOfWeek).HasConversion<string>().HasMaxLength(10);
        builder.Property(sb => sb.SpecificDate);
        builder.Property(sb => sb.BlockType).HasConversion<string>().HasMaxLength(20);
        builder.Property(sb => sb.MaxAppointmentsAllowed);

        builder.OwnsOne(sb => sb.TimeRange, timeRange =>
        {
            timeRange.OwnsOne(t => t.Value, range =>
            {
                range.Property(r => r.Start).HasColumnType("timestamp with time zone").IsRequired();
                range.Property(r => r.End).HasColumnType("timestamp with time zone");
            });
        });

        builder.HasIndex(sb => sb.DoctorScheduleId);
    }
}
