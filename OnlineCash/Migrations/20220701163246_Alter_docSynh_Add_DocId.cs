using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class Alter_docSynh_Add_DocId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocId",
                table: "DocSynches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeDoc",
                table: "DocSynches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isSuccess",
                table: "DocSynches",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocId",
                table: "DocSynches");

            migrationBuilder.DropColumn(
                name: "TypeDoc",
                table: "DocSynches");

            migrationBuilder.DropColumn(
                name: "isSuccess",
                table: "DocSynches");
        }
    }
}
