using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lake7.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeDeliveryDriverIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Drivers_DriverId",
                table: "Deliveries");

            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "Deliveries",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Drivers_DriverId",
                table: "Deliveries",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Drivers_DriverId",
                table: "Deliveries");

            migrationBuilder.AlterColumn<Guid>(
                name: "DriverId",
                table: "Deliveries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Drivers_DriverId",
                table: "Deliveries",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
