using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospitaly.Modules.Clinic.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                schema: "clinics",
                table: "Doctors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                schema: "clinics",
                table: "Doctors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "clinics",
                table: "Doctors",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "clinics",
                table: "Doctors",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                schema: "clinics",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Bio",
                schema: "clinics",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "clinics",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "clinics",
                table: "Doctors");
        }
    }
}
