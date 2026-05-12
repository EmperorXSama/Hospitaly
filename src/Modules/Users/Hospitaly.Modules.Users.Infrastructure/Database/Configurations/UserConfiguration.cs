using Hospitaly.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospitaly.Modules.Users.Infrastructure.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.IdentityId).IsUnique();
        builder.Ignore(u => u.Audit);

        builder.Property(u => u.FirsName)
            .HasMaxLength(100);
        builder.Property(u => u.LastName)
            .HasMaxLength(100);
        builder.Property(u => u.Email)
            .HasMaxLength(200);
        builder.Property(u => u.RequiresOnboarding)
            .HasDefaultValue(true);

        builder.Property(u => u.Sex)
            .HasConversion(
                v => v.Name,
                v => Sex.All.Single(s => s.Name == v))
            .HasMaxLength(10);

        builder.Property(u => u.DateOfBirth)
            .HasColumnType("date");

        builder.Property(u => u.BloodType)
            .HasConversion(
                v => v == null ? null : v.Name,
                v => v == null ? null : BloodType.All.Single(bt => bt.Name == v))
            .HasMaxLength(5)
            .IsRequired(false);

        builder.HasIndex(u => u.Email).IsUnique();
    }
}
