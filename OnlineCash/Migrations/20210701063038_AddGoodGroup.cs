using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AddGoodGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GoodGroupId",
                table: "Goods",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Goods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GoodGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodGroups", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_GoodGroupId",
                table: "Goods",
                column: "GoodGroupId");
            /*
            migrationBuilder.AddForeignKey(
                name: "FK_Goods_GoodGroups_GoodGroupId",
                table: "Goods",
                column: "GoodGroupId",
                principalTable: "GoodGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goods_GoodGroups_GoodGroupId",
                table: "Goods");

            migrationBuilder.DropTable(
                name: "GoodGroups");

            migrationBuilder.DropIndex(
                name: "IX_Goods_GoodGroupId",
                table: "Goods");

            migrationBuilder.DropColumn(
                name: "GoodGroupId",
                table: "Goods");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Goods");
        }
    }
}
