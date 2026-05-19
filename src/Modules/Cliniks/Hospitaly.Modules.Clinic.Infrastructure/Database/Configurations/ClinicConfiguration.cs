using Hospitaly.Modules.Clinic.Domain.Clinic.Entities;
using Hospitaly.Modules.Clinic.Domain.Clinic.ValueObjects;
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

        builder.OwnsMany(c => c.OperatingHours, hours =>
        {
            hours.ToTable("ClinicOperatingHours");

            hours.WithOwner()
                .HasForeignKey("ClinicId");

            hours.Property(h => h.Day)
                .HasColumnName("Day")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            hours.HasKey("ClinicId", nameof(OperatingHours.Day));

            hours.Ignore(h => h.IsOffDay);
            hours.Ignore(h => h.HasRestingTime);

            hours.OwnsOne(h => h.Hours, opHours =>
            {
                opHours.Property<bool>("_isActive")
                    .HasColumnName("HoursActive");

                opHours.OwnsOne(o => o.Value, range =>
                {
                    range.Property(r => r.Start)
                        .HasColumnName("OpenTime")
                        .HasColumnType("timestamp with time zone");

                    range.Property(r => r.End)
                        .HasColumnName("CloseTime")
                        .HasColumnType("timestamp with time zone");
                });
            });

            hours.OwnsOne(h => h.RestingTime, resting =>
            {
                resting.Property<bool>("_isActive")
                    .HasColumnName("RestingTimeActive");

                resting.OwnsOne(o => o.Value, range =>
                {
                    range.Property(r => r.Start)
                        .HasColumnName("RestingStartTime")
                        .HasColumnType("timestamp with time zone");

                    range.Property(r => r.End)
                        .HasColumnName("RestingEndTime")
                        .HasColumnType("timestamp with time zone");
                });
            });
        });
        builder.OwnsOne(c => c.Audit, audit =>
        {
            audit.Property(a => a.CreatedBy)
                .HasColumnName("CreatedBy")
                .IsRequired();
            audit.Property(a => a.CreatedOnUtc)
                .HasColumnName("CreatedOnUtc")
                .HasColumnType("timestamp with time zone")
                .IsRequired();
            audit.Property(a => a.UpdatedBy)
                .HasColumnName("UpdatedBy");
            audit.Property(a => a.UpdatedOnUtc)
                .HasColumnName("UpdatedOnUtc")
                .HasColumnType("timestamp with time zone");
        });
        builder.HasOne(c => c.OperatingLicense)
            .WithOne()
            .HasForeignKey<OperatingLicense>("ClinicId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Ownerships)
            .WithOne()
            .HasForeignKey("ClinicId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(c => c.Ownerships)
            .HasField("_ownerships");

        builder.Navigation(c => c.Ownerships)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
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
