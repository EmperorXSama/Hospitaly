using DoctorScheduleEntity = Hospitaly.Modules.Clinic.Domain.DoctorSchedule.DoctorSchedule;
using Hospitaly.Modules.Clinic.Domain.DoctorSchedule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Configurations;

public class DoctorScheduleConfiguration : IEntityTypeConfiguration<DoctorScheduleEntity>
{
    public void Configure(EntityTypeBuilder<DoctorScheduleEntity> builder)
    {
        builder.ToTable("DoctorSchedules");

        builder.HasKey(ds => ds.Id);

        builder.Ignore(ds => ds.Blocks);

        builder.Property(ds => ds.DoctorId).IsRequired();
        builder.Property(ds => ds.ClinicId).IsRequired();

        builder.OwnsOne(ds => ds.Audit, audit =>
        {
            audit.Property(a => a.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            audit.Property(a => a.CreatedOnUtc).HasColumnName("CreatedOnUtc").HasColumnType("timestamp with time zone").IsRequired();
            audit.Property(a => a.UpdatedBy).HasColumnName("UpdatedBy");
            audit.Property(a => a.UpdatedOnUtc).HasColumnName("UpdatedOnUtc").HasColumnType("timestamp with time zone");
        });

        builder.HasMany<ScheduleBlock>("_blocks")
            .WithOne()
            .HasForeignKey(sb => sb.DoctorScheduleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ds => ds.DoctorId);
        builder.HasIndex(ds => ds.ClinicId);
    }
}
