using PatientEntity = Hospitaly.Modules.Clinic.Domain.Patient.Patient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<PatientEntity>
{
    public void Configure(EntityTypeBuilder<PatientEntity> builder)
    {
        builder.ToTable("Patients");

        builder.HasKey(p => p.Id);

        builder.OwnsOne(p => p.Identity, identity =>
        {
            identity.Property(i => i.FirstName).HasMaxLength(100).IsRequired();
            identity.Property(i => i.LastName).HasMaxLength(100).IsRequired();
            identity.Property(i => i.DateOfBirth).IsRequired();
            identity.Property(i => i.Gender).HasConversion<string>().HasMaxLength(10).IsRequired();
            identity.Property(i => i.NationalId).HasMaxLength(50).IsRequired();

            identity.HasIndex(i => i.NationalId);
        });

        builder.OwnsOne(p => p.Contact, contact =>
        {
            contact.OwnsOne(c => c.PhoneNumber, phone =>
            {
                phone.Property(pn => pn.Value).HasMaxLength(20).HasColumnName("PhoneNumber");
            });

            contact.OwnsOne(c => c.Email, email =>
            {
                email.Property(e => e.Value).HasMaxLength(200).HasColumnName("Email");
            });

            contact.OwnsOne(c => c.Address, address =>
            {
                address.Property(a => a.Street).HasMaxLength(300);
                address.Property(a => a.City).HasMaxLength(100);
                address.Property(a => a.Region).HasMaxLength(100);
                address.Property(a => a.PostalCode).HasMaxLength(20);
                address.Property(a => a.Country).HasMaxLength(100);
            });
        });

        builder.OwnsOne(p => p.PatientType, patientType =>
        {
            patientType.Property(pt => pt.Type).HasConversion<string>().HasMaxLength(20).IsRequired();
            patientType.Property(pt => pt.RegistrationDate).HasColumnType("timestamp with time zone");
        });

        builder.OwnsOne(p => p.Insurance, insurance =>
        {
            insurance.Property(i => i.InsurerName).HasMaxLength(200);
            insurance.Property(i => i.PolicyNumber).HasMaxLength(100);
            insurance.Property(i => i.GroupNumber).HasMaxLength(100);

            insurance.OwnsOne(i => i.Value, range =>
            {
                range.Property(r => r.Start).HasColumnType("timestamp with time zone");
                range.Property(r => r.End).HasColumnType("timestamp with time zone");
            });
        });
    }
}
