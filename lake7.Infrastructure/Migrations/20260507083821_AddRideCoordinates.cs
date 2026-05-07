using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lake7.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRideCoordinates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DropLongitude",
                table: "Rides",
                newName: "DropoffLongitude");

            migrationBuilder.RenameColumn(
                name: "DropLatitude",
                table: "Rides",
                newName: "DropoffLatitude");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DropoffLongitude",
                table: "Rides",
                newName: "DropLongitude");

            migrationBuilder.RenameColumn(
                name: "DropoffLatitude",
                table: "Rides",
                newName: "DropLatitude");
        }
    }
}
