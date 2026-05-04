using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lake7.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeliveryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PickupLocation",
                table: "Deliveries",
                newName: "SenderPhone");

            migrationBuilder.RenameColumn(
                name: "DropoffLocation",
                table: "Deliveries",
                newName: "SenderName");

            migrationBuilder.AddColumn<string>(
                name: "DropoffAddress",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "DropoffLatitude",
                table: "Deliveries",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DropoffLongitude",
                table: "Deliveries",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ItemDescription",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PickupAddress",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "PickupLatitude",
                table: "Deliveries",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PickupLongitude",
                table: "Deliveries",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverName",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverPhone",
                table: "Deliveries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DropoffAddress",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "DropoffLatitude",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "DropoffLongitude",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ItemDescription",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "PickupAddress",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "PickupLatitude",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "PickupLongitude",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ReceiverName",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ReceiverPhone",
                table: "Deliveries");

            migrationBuilder.RenameColumn(
                name: "SenderPhone",
                table: "Deliveries",
                newName: "PickupLocation");

            migrationBuilder.RenameColumn(
                name: "SenderName",
                table: "Deliveries",
                newName: "DropoffLocation");
        }
    }
}
