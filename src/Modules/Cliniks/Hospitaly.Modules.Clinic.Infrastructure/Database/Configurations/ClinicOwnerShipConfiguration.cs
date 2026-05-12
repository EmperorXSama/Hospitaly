using Hospitaly.Modules.Clinic.Domain.Clinic.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Configurations;

public class ClinicOwnerShipConfiguration : IEntityTypeConfiguration<ClinicOwnerShip>
{
    public void Configure(EntityTypeBuilder<ClinicOwnerShip> builder)
    {
        builder.ToTable("ClinicOwnerships");

        builder.HasKey(co => co.Id);

        builder.Property(co => co.OwnerId).IsRequired();
        builder.Property(co => co.OwnerShipType).HasConversion<string>().HasMaxLength(20);
        builder.Property(co => co.SharePercentage).HasColumnType("decimal(5,2)");
        builder.Property(co => co.Status).HasConversion<string>().HasMaxLength(20);

        builder.OwnsOne(co => co.OwnershipEffectivePeriod, period =>
        {
            period.OwnsOne(p => p.Range, range =>
            {
                range.Property(r => r.Start).HasColumnType("timestamp with time zone").IsRequired();
                range.Property(r => r.End).HasColumnType("timestamp with time zone");
            });
        });
    }
}
