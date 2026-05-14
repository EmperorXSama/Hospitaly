using Hospitaly.Modules.Clinic.Domain.Clinic.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Configurations;

public class OperatingLicenseConfiguration : IEntityTypeConfiguration<OperatingLicense>
{
    public void Configure(EntityTypeBuilder<OperatingLicense> builder)
    {
        builder.ToTable("OperatingLicenses");

        builder.HasKey(ol => ol.Id);

        builder.Property(ol => ol.LicenseNumber).HasMaxLength(100).IsRequired();
        builder.Property(ol => ol.IssuingAuthority).HasMaxLength(200);
        builder.Property(ol => ol.LicenseType).HasConversion<string>().HasMaxLength(30);
        builder.Property(ol => ol.AdministrativeStatus).HasConversion<string>().HasMaxLength(20);

        builder.OwnsOne(ol => ol.ValidityPeriod, validity =>
        {
            validity.OwnsOne(v => v.Value, range =>
            {
                range.Property(r => r.Start).HasColumnType("timestamp with time zone").IsRequired();
                range.Property(r => r.End).HasColumnType("timestamp with time zone");
            });
        });

        builder.Property<Guid>("ClinicId");

        builder.OwnsOne(ol => ol.Audit, audit =>
        {
            audit.Property(a => a.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            audit.Property(a => a.CreatedOnUtc).HasColumnName("CreatedOnUtc").HasColumnType("timestamp with time zone").IsRequired();
            audit.Property(a => a.UpdatedBy).HasColumnName("UpdatedBy");
            audit.Property(a => a.UpdatedOnUtc).HasColumnName("UpdatedOnUtc").HasColumnType("timestamp with time zone");
        });

        builder.HasIndex("ClinicId").IsUnique();
    }
}
