using Hospitaly.Modules.Clinic.Domain.Doctor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Configurations;

public class DoctorSpecialtyConfiguration : IEntityTypeConfiguration<DoctorSpecialty>
{
    public void Configure(EntityTypeBuilder<DoctorSpecialty> builder)
    {
        builder.ToTable("DoctorSpecialties");

        builder.HasKey(ds => new { ds.DoctorId, ds.SpecialtyId });

        builder.Property(ds => ds.IsPrimary);
        builder.Property(ds => ds.CertificationNumber).HasMaxLength(100);
        builder.Property(ds => ds.CertifiedAt).HasColumnType("timestamp with time zone").IsRequired();

        builder.HasOne(ds => ds.Doctor)
            .WithMany()
            .HasForeignKey(ds => ds.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ds => ds.Specialty)
            .WithMany()
            .HasForeignKey(ds => ds.SpecialtyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
