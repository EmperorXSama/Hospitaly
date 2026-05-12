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

        builder.HasMany<ScheduleBlock>("_blocks")
            .WithOne()
            .HasForeignKey(sb => sb.DoctorScheduleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ds => ds.DoctorId);
        builder.HasIndex(ds => ds.ClinicId);
    }
}
