using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkAPI.Migrations
{
    public partial class addState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "NationalParks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "NationalParks");
        }
    }
}
