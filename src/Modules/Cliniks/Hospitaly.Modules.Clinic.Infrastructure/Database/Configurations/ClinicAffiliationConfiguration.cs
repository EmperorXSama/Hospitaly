using Hospitaly.Modules.Clinic.Domain.Doctor;
using Hospitaly.Modules.Clinic.Domain.Doctor.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Configurations;

public class ClinicAffiliationConfiguration : IEntityTypeConfiguration<ClinicAffiliation>
{
    public void Configure(EntityTypeBuilder<ClinicAffiliation> builder)
    {
        builder.ToTable("ClinicAffiliations");

        builder.HasKey(ca => ca.Id);

        builder.Ignore(ca => ca.GrantedPrivileges);

        builder.Property(ca => ca.ClinicId).IsRequired();
        builder.Property(ca => ca.DoctorId).IsRequired();
        builder.Property(ca => ca.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(ca => ca.JoinedDate).HasColumnType("timestamp with time zone").IsRequired();
        builder.Property(ca => ca.TerminatedDate).HasColumnType("timestamp with time zone");
        builder.Property(ca => ca.DepartmentId);

        builder.OwnsMany<Privilege>("_grantedPrivileges", privileges =>
        {
            privileges.ToTable("ClinicAffiliationPrivileges");
            privileges.WithOwner().HasForeignKey("ClinicAffiliationId");

            privileges.Property(p => p.Type).HasConversion<string>().HasMaxLength(30).IsRequired();
            privileges.Property(p => p.GrantedAt).HasColumnType("timestamp with time zone").IsRequired();
            privileges.Property(p => p.GrantedBy).IsRequired();
        });

        builder.OwnsOne(ca => ca.Audit, audit =>
        {
            audit.Property(a => a.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            audit.Property(a => a.CreatedOnUtc).HasColumnName("CreatedOnUtc").HasColumnType("timestamp with time zone").IsRequired();
            audit.Property(a => a.UpdatedBy).HasColumnName("UpdatedBy");
            audit.Property(a => a.UpdatedOnUtc).HasColumnName("UpdatedOnUtc").HasColumnType("timestamp with time zone");
        });

        builder.HasOne(ca => ca.Doctor)
            .WithMany()
            .HasForeignKey(ca => ca.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ca => new { ca.ClinicId, ca.DoctorId });
    }
}
