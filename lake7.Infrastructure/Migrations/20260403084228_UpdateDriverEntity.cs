using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lake7.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDriverEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompletedRides",
                table: "Drivers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Drivers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActiveAt",
                table: "Drivers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicensePlate",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Drivers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Drivers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleType",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedRides",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "LastActiveAt",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "LicensePlate",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "VehicleType",
                table: "Drivers");
        }
    }
}
