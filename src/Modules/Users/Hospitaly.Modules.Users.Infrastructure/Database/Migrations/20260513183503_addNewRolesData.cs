using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospitaly.Modules.Users.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class addNewRolesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "users",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "clinics:ownership:transfer", "Member" });

            migrationBuilder.InsertData(
                schema: "users",
                table: "Roles",
                column: "Name",
                values: new object[]
                {
                    "Doctor",
                    "HospitalAdministrator",
                    "Patient"
                });

            migrationBuilder.InsertData(
                schema: "users",
                table: "RolePermissions",
                columns: new[] { "PermissionCode", "RoleName" },
                values: new object[] { "clinics:ownership:transfer", "HospitalAdministrator" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "users",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "clinics:ownership:transfer", "HospitalAdministrator" });

            migrationBuilder.DeleteData(
                schema: "users",
                table: "Roles",
                keyColumn: "Name",
                keyValue: "Doctor");

            migrationBuilder.DeleteData(
                schema: "users",
                table: "Roles",
                keyColumn: "Name",
                keyValue: "Patient");

            migrationBuilder.DeleteData(
                schema: "users",
                table: "Roles",
                keyColumn: "Name",
                keyValue: "HospitalAdministrator");

            migrationBuilder.InsertData(
                schema: "users",
                table: "RolePermissions",
                columns: new[] { "PermissionCode", "RoleName" },
                values: new object[] { "clinics:ownership:transfer", "Member" });
        }
    }
}
