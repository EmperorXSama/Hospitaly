using StaffMemberEntity = Hospitaly.Modules.Clinic.Domain.StaffMember.StaffMember;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Configurations;

public class StaffMemberConfiguration : IEntityTypeConfiguration<StaffMemberEntity>
{
    public void Configure(EntityTypeBuilder<StaffMemberEntity> builder)
    {
        builder.ToTable("StaffMembers");

        builder.HasKey(sm => sm.Id);

        builder.Property(sm => sm.IdentityId).HasMaxLength(100).IsRequired();
        builder.Property(sm => sm.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(sm => sm.LastName).HasMaxLength(100).IsRequired();
        builder.Property(sm => sm.Phone).HasMaxLength(20);
        builder.Property(sm => sm.Email).HasMaxLength(200);

        builder.OwnsOne(sm => sm.Role, role =>
        {
            role.Property(r => r.Role).HasConversion<string>().HasMaxLength(30).IsRequired();
            role.Property(r => r.Department).HasMaxLength(200).IsRequired();
        });

        builder.OwnsOne(sm => sm.Employment, employment =>
        {
            employment.Property(e => e.HireDate).HasColumnType("timestamp with time zone").IsRequired();
            employment.Property(e => e.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
            employment.Property(e => e.ContractType).HasConversion<string>().HasMaxLength(20).IsRequired();
        });

        builder.OwnsOne(sm => sm.Audit, audit =>
        {
            audit.Property(a => a.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            audit.Property(a => a.CreatedOnUtc).HasColumnName("CreatedOnUtc").HasColumnType("timestamp with time zone").IsRequired();
            audit.Property(a => a.UpdatedBy).HasColumnName("UpdatedBy");
            audit.Property(a => a.UpdatedOnUtc).HasColumnName("UpdatedOnUtc").HasColumnType("timestamp with time zone");
        });

        builder.HasIndex(sm => sm.IdentityId).IsUnique();
    }
}
