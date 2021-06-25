using Microsoft.EntityFrameworkCore.Migrations;

namespace RMK.Migrations
{
    public partial class AlterCheckIsElectron : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsElectron",
                table: "CheckSells",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsElectron",
                table: "CheckSells");
        }
    }
}
