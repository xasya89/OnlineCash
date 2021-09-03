using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AlterWriteogAddColumn_Status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Writeofs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Writeofs");
        }
    }
}
