using Hospitaly.Modules.Clinic.Domain.Doctor;
using DoctorEntity = Hospitaly.Modules.Clinic.Domain.Doctor.Doctor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<DoctorEntity>
{
    public void Configure(EntityTypeBuilder<DoctorEntity> builder)
    {
        builder.ToTable("Doctors");

        builder.HasKey(d => d.Id);

        builder.Ignore(d => d.Credentials);
        builder.Ignore(d => d.Specialties);
        builder.Ignore(d => d.Affiliations);

        builder.HasMany<DoctorCredential>("_credentials")
            .WithOne()
            .HasForeignKey(dc => dc.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<DoctorSpecialty>("_specialties")
            .WithOne(ds => ds.Doctor)
            .HasForeignKey(ds => ds.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<ClinicAffiliation>("_affiliations")
            .WithOne(ca => ca.Doctor)
            .HasForeignKey(ca => ca.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
