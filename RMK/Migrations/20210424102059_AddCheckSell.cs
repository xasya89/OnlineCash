using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RMK.Migrations
{
    public partial class AddCheckSell : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckSells",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Sum = table.Column<decimal>(nullable: false),
                    SumDiscont = table.Column<decimal>(nullable: false),
                    SumAll = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckSells", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CheckGoods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Count = table.Column<double>(nullable: false),
                    Cost = table.Column<decimal>(nullable: false),
                    GoodId = table.Column<int>(nullable: false),
                    CheckSellId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckGoods_CheckSells_CheckSellId",
                        column: x => x.CheckSellId,
                        principalTable: "CheckSells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckGoods_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckGoods_CheckSellId",
                table: "CheckGoods",
                column: "CheckSellId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckGoods_GoodId",
                table: "CheckGoods",
                column: "GoodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckGoods");

            migrationBuilder.DropTable(
                name: "CheckSells");
        }
    }
}
