using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class editAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedOnUtc",
                schema: "clinics",
                table: "StaffMembers",
                newName: "UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                schema: "clinics",
                table: "StaffMembers",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedOnUtc",
                schema: "clinics",
                table: "StaffMembers",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                schema: "clinics",
                table: "StaffMembers",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedOnUtc",
                schema: "clinics",
                table: "Specialties",
                newName: "UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                schema: "clinics",
                table: "Specialties",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedOnUtc",
                schema: "clinics",
                table: "Specialties",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                schema: "clinics",
                table: "Specialties",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedOnUtc",
                schema: "clinics",
                table: "ScheduleBlocks",
                newName: "UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                schema: "clinics",
                table: "ScheduleBlocks",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedOnUtc",
                schema: "clinics",
                table: "ScheduleBlocks",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                schema: "clinics",
                table: "ScheduleBlocks",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedOnUtc",
                schema: "clinics",
                table: "Rooms",
                newName: "UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                schema: "clinics",
                table: "Rooms",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedOnUtc",
                schema: "clinics",
                table: "Rooms",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                schema: "clinics",
                table: "Rooms",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedOnUtc",
                schema: "clinics",
                table: "Patients",
                newName: "UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                schema: "clinics",
                table: "Patients",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedOnUtc",
                schema: "clinics",
                table: "Patients",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                schema: "clinics",
                table: "Patients",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedOnUtc",
                schema: "clinics",
                table: "OperatingLicenses",
                newName: "UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                schema: "clinics",
                table: "OperatingLicenses",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedOnUtc",
                schema: "clinics",
                table: "OperatingLicenses",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                schema: "clinics",
                table: "OperatingLicenses",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedOnUtc",
                schema: "clinics",
                table: "MaintenanceBlocks",
                newName: "UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                schema: "clinics",
                table: "MaintenanceBlocks",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedOnUtc",
                schema: "clinics",
                table: "MaintenanceBlocks",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                schema: "clinics",
                table: "MaintenanceBlocks",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedOnUtc",
                schema: "clinics",
                table: "DoctorSchedules",
                newName: "UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                schema: "clinics",
                table: "DoctorSchedules",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedOnUtc",
                schema: "clinics",
                table: "DoctorSchedules",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                schema: "clinics",
                table: "DoctorSchedules",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedOnUtc",
                schema: "clinics",
                table: "Doctors",
                newName: "UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                schema: "clinics",
                table: "Doctors",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedOnUtc",
                schema: "clinics",
                table: "Doctors",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                schema: "clinics",
                table: "Doctors",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedOnUtc",
                schema: "clinics",
                table: "DoctorCredentials",
                newName: "UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                schema: "clinics",
                table: "DoctorCredentials",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedOnUtc",
                schema: "clinics",
                table: "DoctorCredentials",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                schema: "clinics",
                table: "DoctorCredentials",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedOnUtc",
                schema: "clinics",
                table: "Departments",
                newName: "UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                schema: "clinics",
                table: "Departments",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedOnUtc",
                schema: "clinics",
                table: "Departments",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                schema: "clinics",
                table: "Departments",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedOnUtc",
                schema: "clinics",
                table: "ClinicAffiliations",
                newName: "UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                schema: "clinics",
                table: "ClinicAffiliations",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedOnUtc",
                schema: "clinics",
                table: "ClinicAffiliations",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                schema: "clinics",
                table: "ClinicAffiliations",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedOnUtc",
                schema: "clinics",
                table: "Appointments",
                newName: "UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                schema: "clinics",
                table: "Appointments",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedOnUtc",
                schema: "clinics",
                table: "Appointments",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                schema: "clinics",
                table: "Appointments",
                newName: "CreatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedOnUtc",
                schema: "clinics",
                table: "StaffMembers",
                newName: "Audit_UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "clinics",
                table: "StaffMembers",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                schema: "clinics",
                table: "StaffMembers",
                newName: "Audit_CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "clinics",
                table: "StaffMembers",
                newName: "Audit_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedOnUtc",
                schema: "clinics",
                table: "Specialties",
                newName: "Audit_UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "clinics",
                table: "Specialties",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                schema: "clinics",
                table: "Specialties",
                newName: "Audit_CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "clinics",
                table: "Specialties",
                newName: "Audit_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedOnUtc",
                schema: "clinics",
                table: "ScheduleBlocks",
                newName: "Audit_UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "clinics",
                table: "ScheduleBlocks",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                schema: "clinics",
                table: "ScheduleBlocks",
                newName: "Audit_CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "clinics",
                table: "ScheduleBlocks",
                newName: "Audit_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedOnUtc",
                schema: "clinics",
                table: "Rooms",
                newName: "Audit_UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "clinics",
                table: "Rooms",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                schema: "clinics",
                table: "Rooms",
                newName: "Audit_CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "clinics",
                table: "Rooms",
                newName: "Audit_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedOnUtc",
                schema: "clinics",
                table: "Patients",
                newName: "Audit_UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "clinics",
                table: "Patients",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                schema: "clinics",
                table: "Patients",
                newName: "Audit_CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "clinics",
                table: "Patients",
                newName: "Audit_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedOnUtc",
                schema: "clinics",
                table: "OperatingLicenses",
                newName: "Audit_UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "clinics",
                table: "OperatingLicenses",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                schema: "clinics",
                table: "OperatingLicenses",
                newName: "Audit_CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "clinics",
                table: "OperatingLicenses",
                newName: "Audit_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedOnUtc",
                schema: "clinics",
                table: "MaintenanceBlocks",
                newName: "Audit_UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "clinics",
                table: "MaintenanceBlocks",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                schema: "clinics",
                table: "MaintenanceBlocks",
                newName: "Audit_CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "clinics",
                table: "MaintenanceBlocks",
                newName: "Audit_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedOnUtc",
                schema: "clinics",
                table: "DoctorSchedules",
                newName: "Audit_UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "clinics",
                table: "DoctorSchedules",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                schema: "clinics",
                table: "DoctorSchedules",
                newName: "Audit_CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "clinics",
                table: "DoctorSchedules",
                newName: "Audit_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedOnUtc",
                schema: "clinics",
                table: "Doctors",
                newName: "Audit_UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "clinics",
                table: "Doctors",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                schema: "clinics",
                table: "Doctors",
                newName: "Audit_CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "clinics",
                table: "Doctors",
                newName: "Audit_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedOnUtc",
                schema: "clinics",
                table: "DoctorCredentials",
                newName: "Audit_UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "clinics",
                table: "DoctorCredentials",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                schema: "clinics",
                table: "DoctorCredentials",
                newName: "Audit_CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "clinics",
                table: "DoctorCredentials",
                newName: "Audit_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedOnUtc",
                schema: "clinics",
                table: "Departments",
                newName: "Audit_UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "clinics",
                table: "Departments",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                schema: "clinics",
                table: "Departments",
                newName: "Audit_CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "clinics",
                table: "Departments",
                newName: "Audit_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedOnUtc",
                schema: "clinics",
                table: "ClinicAffiliations",
                newName: "Audit_UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "clinics",
                table: "ClinicAffiliations",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                schema: "clinics",
                table: "ClinicAffiliations",
                newName: "Audit_CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "clinics",
                table: "ClinicAffiliations",
                newName: "Audit_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedOnUtc",
                schema: "clinics",
                table: "Appointments",
                newName: "Audit_UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "clinics",
                table: "Appointments",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                schema: "clinics",
                table: "Appointments",
                newName: "Audit_CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "clinics",
                table: "Appointments",
                newName: "Audit_CreatedBy");
        }
    }
}
