using Hospitaly.Modules.Clinic.Domain.Clinic.Entities;
using ClinicEntity = Hospitaly.Modules.Clinic.Domain.Clinic.Clinic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Configurations;

public class ClinicConfiguration : IEntityTypeConfiguration<ClinicEntity>
{
    public void Configure(EntityTypeBuilder<ClinicEntity> builder)
    {
        builder.ToTable("Clinics");

        builder.HasKey(c => c.Id);

        builder.Ignore(c => c.Specialties);
        builder.Ignore(c => c.Departments);
        builder.Ignore(c => c.Ownerships);

        builder.OwnsOne(c => c.Info, info =>
        {
            info.Property(i => i.Name).HasMaxLength(200);
            info.Property(i => i.TradingName).HasMaxLength(200);
            info.Property(i => i.Description).HasMaxLength(1000);
            info.Property(i => i.LogoUrl).HasMaxLength(500);
        });

        builder.OwnsOne(c => c.Address, address =>
        {
            address.OwnsOne(a => a.Value, addr =>
            {
                addr.Property(a => a.Street).HasMaxLength(300);
                addr.Property(a => a.City).HasMaxLength(100);
                addr.Property(a => a.Region).HasMaxLength(100);
                addr.Property(a => a.PostalCode).HasMaxLength(20);
                addr.Property(a => a.Country).HasMaxLength(100);
            });

            address.OwnsOne(a => a.Coordinates, coord =>
            {
                coord.Property(c => c.Latitude).HasColumnType("double precision");
                coord.Property(c => c.Longitude).HasColumnType("double precision");
            });
        });

        builder.OwnsOne(c => c.ContactInfo, contact =>
        {
            contact.OwnsOne(co => co.PhoneNumber, phone =>
            {
                phone.Property(p => p.Value).HasMaxLength(20).HasColumnName("PhoneNumber");
            });

            contact.OwnsOne(co => co.Email, email =>
            {
                email.Property(e => e.Value).HasMaxLength(200).HasColumnName("Email");
            });

            contact.Property(c => c.Website).HasMaxLength(500);
        });

        builder.OwnsOne(c => c.OperatingHours, hours =>
        {
            hours.Property(h => h.Day).HasConversion<string>().HasMaxLength(20);
            hours.Property(h => h.IsResting);

            hours.OwnsOne(h => h.Hours, opHours =>
            {
                opHours.Property("_isActive").HasColumnName("hours_active");

                opHours.OwnsOne(o => o.Value, range =>
                {
                    range.Property(r => r.Start).HasColumnType("timestamp with time zone");
                    range.Property(r => r.End).HasColumnType("timestamp with time zone");
                });
            });
        });

        builder.HasOne(c => c.OperatingLicense)
            .WithOne()
            .HasForeignKey<OperatingLicense>("ClinicId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<ClinicOwnerShip>("_ownerships")
            .WithOne()
            .HasForeignKey("ClinicId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<Department>("_departments")
            .WithOne(d => d.Clinic)
            .HasForeignKey("ClinicId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<ClinicSpecialty>("_specialties")
            .WithOne(cs => cs.Clinic)
            .HasForeignKey(cs => cs.ClinicId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
