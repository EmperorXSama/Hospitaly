using SpecialtyEntity = Hospitaly.Modules.Clinic.Domain.Specialty.Specialty;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Configurations;

public class SpecialtyConfiguration : IEntityTypeConfiguration<SpecialtyEntity>
{
    public void Configure(EntityTypeBuilder<SpecialtyEntity> builder)
    {
        builder.ToTable("Specialties");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name).HasMaxLength(200).IsRequired();
        builder.Property(s => s.ParentId);

        builder.HasOne(s => s.Parent)
            .WithMany(s => s.Children)
            .HasForeignKey(s => s.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => s.Name);
    }
}
