using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RattingSystem.Migrations
{
    public partial class ChangePresenterPropName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Presenter",
                table: "Session",
                newName: "PresenterName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PresenterName",
                table: "Session",
                newName: "Presenter");
        }
    }
}
