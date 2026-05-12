using Hospitaly.Modules.Clinic.Domain.Clinic.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Configurations;

public class ClinicSpecialtyConfiguration : IEntityTypeConfiguration<ClinicSpecialty>
{
    public void Configure(EntityTypeBuilder<ClinicSpecialty> builder)
    {
        builder.ToTable("ClinicSpecialties");

        builder.HasKey(cs => new { cs.ClinicId, cs.SpecialtyId });

        builder.Property(cs => cs.IsActive);
        builder.Property(cs => cs.ConsultationFee).HasColumnType("decimal(10,2)");

        builder.HasOne(cs => cs.Clinic)
            .WithMany()
            .HasForeignKey(cs => cs.ClinicId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cs => cs.Specialty)
            .WithMany()
            .HasForeignKey(cs => cs.SpecialtyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
