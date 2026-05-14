using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class FixAuditMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedOnUtc",
                schema: "clinics",
                table: "Clinics",
                newName: "UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                schema: "clinics",
                table: "Clinics",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedOnUtc",
                schema: "clinics",
                table: "Clinics",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                schema: "clinics",
                table: "Clinics",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedOnUtc",
                schema: "clinics",
                table: "ClinicOwnerships",
                newName: "UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_UpdatedBy",
                schema: "clinics",
                table: "ClinicOwnerships",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedOnUtc",
                schema: "clinics",
                table: "ClinicOwnerships",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "Audit_CreatedBy",
                schema: "clinics",
                table: "ClinicOwnerships",
                newName: "CreatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedOnUtc",
                schema: "clinics",
                table: "Clinics",
                newName: "Audit_UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "clinics",
                table: "Clinics",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                schema: "clinics",
                table: "Clinics",
                newName: "Audit_CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "clinics",
                table: "Clinics",
                newName: "Audit_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedOnUtc",
                schema: "clinics",
                table: "ClinicOwnerships",
                newName: "Audit_UpdatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "clinics",
                table: "ClinicOwnerships",
                newName: "Audit_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                schema: "clinics",
                table: "ClinicOwnerships",
                newName: "Audit_CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "clinics",
                table: "ClinicOwnerships",
                newName: "Audit_CreatedBy");
        }
    }
}
