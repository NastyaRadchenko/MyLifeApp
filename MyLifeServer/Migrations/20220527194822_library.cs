using Microsoft.EntityFrameworkCore.Migrations;

namespace MyLifeServer.Migrations
{
    public partial class library : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EntryId",
                table: "BookCategories",
                newName: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "BookCategories",
                newName: "EntryId");
        }
    }
}
