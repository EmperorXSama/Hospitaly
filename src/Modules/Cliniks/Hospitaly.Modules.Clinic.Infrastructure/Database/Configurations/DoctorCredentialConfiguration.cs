using Hospitaly.Modules.Clinic.Domain.Doctor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Configurations;

public class DoctorCredentialConfiguration : IEntityTypeConfiguration<DoctorCredential>
{
    public void Configure(EntityTypeBuilder<DoctorCredential> builder)
    {
        builder.ToTable("DoctorCredentials");

        builder.HasKey(dc => dc.Id);

        builder.Property(dc => dc.DoctorId).IsRequired();
        builder.Property(dc => dc.CredentialType).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(dc => dc.IssuingAuthority).HasMaxLength(200).IsRequired();
        builder.Property(dc => dc.DocumentNumber).HasMaxLength(100).IsRequired();
        builder.Property(dc => dc.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(dc => dc.VerifiedAt).HasColumnType("timestamp with time zone");
        builder.Property(dc => dc.VerifiedBy);

        builder.OwnsOne(dc => dc.ValidityPeriod, validity =>
        {
            validity.OwnsOne(v => v.Value, range =>
            {
                range.Property(r => r.Start).HasColumnType("timestamp with time zone").IsRequired();
                range.Property(r => r.End).HasColumnType("timestamp with time zone");
            });
        });

        builder.HasIndex(dc => dc.DoctorId);
        builder.HasIndex(dc => dc.DocumentNumber);
    }
}
