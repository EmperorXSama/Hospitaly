using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospitaly.Modules.Users.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class addHospitapPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "users",
                table: "Permissions",
                column: "Code",
                value: "clinics:ownership:transfer");

            migrationBuilder.InsertData(
                schema: "users",
                table: "RolePermissions",
                columns: new[] { "PermissionCode", "RoleName" },
                values: new object[] { "clinics:ownership:transfer", "Member" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "users",
                table: "RolePermissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "clinics:ownership:transfer", "Member" });

            migrationBuilder.DeleteData(
                schema: "users",
                table: "Permissions",
                keyColumn: "Code",
                keyValue: "clinics:ownership:transfer");
        }
    }
}
