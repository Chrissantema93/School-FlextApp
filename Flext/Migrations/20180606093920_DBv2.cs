using Microsoft.EntityFrameworkCore.Migrations;

namespace Flext.Migrations
{
    public partial class DBv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoelId",
                table: "Detecties",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoelId",
                table: "Detecties");
        }
    }
}
