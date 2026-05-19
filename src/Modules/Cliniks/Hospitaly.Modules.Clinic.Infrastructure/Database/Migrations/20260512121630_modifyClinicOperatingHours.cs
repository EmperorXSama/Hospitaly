using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class modifyClinicOperatingHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperatingHours_Day",
                schema: "clinics",
                table: "Clinics");

            migrationBuilder.DropColumn(
                name: "OperatingHours_Hours_Value_End",
                schema: "clinics",
                table: "Clinics");

            migrationBuilder.DropColumn(
                name: "OperatingHours_Hours_Value_Start",
                schema: "clinics",
                table: "Clinics");

            migrationBuilder.DropColumn(
                name: "OperatingHours_IsResting",
                schema: "clinics",
                table: "Clinics");

            migrationBuilder.DropColumn(
                name: "hours_active",
                schema: "clinics",
                table: "Clinics");

            migrationBuilder.CreateTable(
                name: "ClinicOperatingHours",
                schema: "clinics",
                columns: table => new
                {
                    Day = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ClinicId = table.Column<Guid>(type: "uuid", nullable: false),
                    OpenTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CloseTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    HoursActive = table.Column<bool>(type: "boolean", nullable: true),
                    RestingStartTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RestingEndTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RestingTimeActive = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicOperatingHours", x => new { x.ClinicId, x.Day });
                    table.ForeignKey(
                        name: "FK_ClinicOperatingHours_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalSchema: "clinics",
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClinicOperatingHours",
                schema: "clinics");

            migrationBuilder.AddColumn<string>(
                name: "OperatingHours_Day",
                schema: "clinics",
                table: "Clinics",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "OperatingHours_Hours_Value_End",
                schema: "clinics",
                table: "Clinics",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "OperatingHours_Hours_Value_Start",
                schema: "clinics",
                table: "Clinics",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OperatingHours_IsResting",
                schema: "clinics",
                table: "Clinics",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "hours_active",
                schema: "clinics",
                table: "Clinics",
                type: "boolean",
                nullable: true);
        }
    }
}
