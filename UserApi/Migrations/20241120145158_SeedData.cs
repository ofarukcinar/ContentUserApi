using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 11, 20, 14, 51, 58, 27, DateTimeKind.Utc).AddTicks(5930), "Admin" },
                    { 2, new DateTime(2024, 11, 20, 14, 51, 58, 27, DateTimeKind.Utc).AddTicks(5940), "User" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Name", "RoleId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 11, 20, 14, 51, 58, 27, DateTimeKind.Utc).AddTicks(6040), "john@example.com", "John Doe", 1 },
                    { 2, new DateTime(2024, 11, 20, 14, 51, 58, 27, DateTimeKind.Utc).AddTicks(6050), "jane@example.com", "Jane Doe", 2 }
                });

            migrationBuilder.InsertData(
                table: "UserDetails",
                columns: new[] { "Id", "Address", "CreatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, "123 Main St", new DateTime(2024, 11, 20, 14, 51, 58, 27, DateTimeKind.Utc).AddTicks(6070), 1 },
                    { 2, "456 Elm St", new DateTime(2024, 11, 20, 14, 51, 58, 27, DateTimeKind.Utc).AddTicks(6070), 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserDetails",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserDetails",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
