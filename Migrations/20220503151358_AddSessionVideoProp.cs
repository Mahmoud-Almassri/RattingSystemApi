using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RattingSystem.Migrations
{
    public partial class AddSessionVideoProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SessionVideo",
                table: "Session",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionVideo",
                table: "Session");
        }
    }
}
