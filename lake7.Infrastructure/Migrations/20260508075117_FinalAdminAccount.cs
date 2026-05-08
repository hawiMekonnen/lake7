using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lake7.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinalAdminAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminAccounts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AdminAccounts",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "LastLogin", "Password", "Role", "UpdatedAt" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2024, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@lake7.com", "System Admin", new DateTime(2024, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin123", "SuperAdmin", new DateTime(2024, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminAccounts");
        }
    }
}
