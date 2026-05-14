using Hospitaly.Modules.Clinic.Domain.Appointment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.DoctorId).IsRequired();
        builder.Property(a => a.PatientId).IsRequired();
        builder.Property(a => a.ClinicId).IsRequired();
        builder.Property(a => a.RoomId);

        builder.OwnsOne(a => a.TimeSlot, timeSlot =>
        {
            timeSlot.OwnsOne(t => t.DateTimeRange, range =>
            {
                range.Property(r => r.Start).HasColumnType("timestamp with time zone").IsRequired();
                range.Property(r => r.End).HasColumnType("timestamp with time zone");
            });
        });

        builder.OwnsOne(a => a.AppointmentType, appType =>
        {
            appType.Property(t => t.Type).HasConversion<string>().HasMaxLength(50);
            appType.Property(t => t.ExpectedDuration).HasColumnType("interval");
        });

        builder.OwnsOne(a => a.Status, status =>
        {
            status.Property(s => s.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
            status.Property(s => s.SetAt).HasColumnType("timestamp with time zone").IsRequired();
        });

        builder.OwnsOne(a => a.Cancellation, cancellation =>
        {
            cancellation.Property(c => c.Reason).HasConversion<string>().HasMaxLength(50);
            cancellation.Property(c => c.InitiatedBy).HasConversion<string>().HasMaxLength(20);
            cancellation.Property(c => c.Notes).HasMaxLength(500);
            cancellation.Property(c => c.CancelledAt).HasColumnType("timestamp with time zone");
        });

        builder.OwnsOne(a => a.RescheduleInfo, reschedule =>
        {
            reschedule.Property(r => r.Reason).HasMaxLength(500);
            reschedule.Property(r => r.RescheduledAt).HasColumnType("timestamp with time zone");
            reschedule.Property(r => r.RequestedBy).HasConversion<string>().HasMaxLength(20);

            reschedule.OwnsOne(r => r.OriginalTimeSlot, original =>
            {
                original.OwnsOne(o => o.DateTimeRange, range =>
                {
                    range.Property(r => r.Start).HasColumnType("timestamp with time zone");
                    range.Property(r => r.End).HasColumnType("timestamp with time zone");
                });
            });
        });

        builder.OwnsOne(a => a.Audit, audit =>
        {
            audit.Property(a => a.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            audit.Property(a => a.CreatedOnUtc).HasColumnName("CreatedOnUtc").HasColumnType("timestamp with time zone").IsRequired();
            audit.Property(a => a.UpdatedBy).HasColumnName("UpdatedBy");
            audit.Property(a => a.UpdatedOnUtc).HasColumnName("UpdatedOnUtc").HasColumnType("timestamp with time zone");
        });

        builder.HasIndex(a => a.DoctorId);
        builder.HasIndex(a => a.PatientId);
        builder.HasIndex(a => a.ClinicId);
        builder.HasIndex(a => new { a.DoctorId, a.ClinicId });
    }
}
