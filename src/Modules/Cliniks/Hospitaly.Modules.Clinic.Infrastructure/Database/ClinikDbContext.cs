using Hospitaly.Common.Domain;
using Hospitaly.Modules.Clinic.Application.Abstractions.Data;
using Hospitaly.Modules.Clinic.Domain.Appointment;
using Hospitaly.Modules.Clinic.Domain.Clinic.Entities;
using Hospitaly.Modules.Clinic.Domain.Doctor;
using Hospitaly.Modules.Clinic.Domain.DoctorSchedule;
using Hospitaly.Modules.Clinic.Domain.Patient;
using Hospitaly.Modules.Clinic.Domain.Room;
using Hospitaly.Modules.Clinic.Domain.Specialty;
using Hospitaly.Modules.Clinic.Domain.StaffMember;
using ClinicEntity = Hospitaly.Modules.Clinic.Domain.Clinic.Clinic;
using Microsoft.EntityFrameworkCore;

namespace Hospitaly.Modules.Clinic.Infrastructure.Database;

public class ClinikDbContext(DbContextOptions<ClinikDbContext> options) : DbContext(options), IUnitOfWork
{
    internal DbSet<ClinicEntity> Clinics => Set<ClinicEntity>();
    internal DbSet<Appointment> Appointments => Set<Appointment>();
    internal DbSet<Doctor> Doctors => Set<Doctor>();
    internal DbSet<DoctorSchedule> DoctorSchedules => Set<DoctorSchedule>();
    internal DbSet<Patient> Patients => Set<Patient>();
    internal DbSet<Room> Rooms => Set<Room>();
    internal DbSet<StaffMember> StaffMembers => Set<StaffMember>();

    internal DbSet<OperatingLicense> OperatingLicenses => Set<OperatingLicense>();
    internal DbSet<ClinicOwnerShip> ClinicOwnerships => Set<ClinicOwnerShip>();
    internal DbSet<Department> Departments => Set<Department>();
    internal DbSet<ClinicSpecialty> ClinicSpecialties => Set<ClinicSpecialty>();
    internal DbSet<ClinicAffiliation> ClinicAffiliations => Set<ClinicAffiliation>();
    internal DbSet<DoctorCredential> DoctorCredentials => Set<DoctorCredential>();
    internal DbSet<DoctorSpecialty> DoctorSpecialties => Set<DoctorSpecialty>();
    internal DbSet<ScheduleBlock> ScheduleBlocks => Set<ScheduleBlock>();
    internal DbSet<MaintenanceBlock> MaintenanceBlocks => Set<MaintenanceBlock>();
    internal DbSet<Specialty> Specialties => Set<Specialty>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Clinic);
        modelBuilder.Ignore<DomainEvent>();
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes().ToList())
        {
            if (!typeof(Entity).IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            modelBuilder.Entity(entityType.ClrType).OwnsOne(typeof(AuditInfo), "Audit", audit =>
            {
                audit.Property("CreatedBy").IsRequired();
                audit.Property("CreatedOnUtc").HasColumnType("timestamp with time zone").IsRequired();
                audit.Property("UpdatedBy");
                audit.Property("UpdatedOnUtc").HasColumnType("timestamp with time zone");
            });
        }
    }
}