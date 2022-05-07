using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RattingSystem.Migrations
{
    public partial class PresenteRaterDegree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RateDegree",
                table: "Rate",
                newName: "SessionRateDegree");

            migrationBuilder.AddColumn<int>(
                name: "PresenteRaterDegree",
                table: "Rate",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PresenteRaterDegree",
                table: "Rate");

            migrationBuilder.RenameColumn(
                name: "SessionRateDegree",
                table: "Rate",
                newName: "RateDegree");
        }
    }
}
