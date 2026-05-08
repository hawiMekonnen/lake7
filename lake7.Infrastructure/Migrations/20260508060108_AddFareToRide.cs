using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lake7.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFareToRide : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Fare",
                table: "Rides",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Fare",
                table: "Deliveries",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fare",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "Fare",
                table: "Deliveries");
        }
    }
}
