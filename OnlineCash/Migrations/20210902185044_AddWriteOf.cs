using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AddWriteOf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Writeofs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DateCreate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ShopId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "longtext", nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    SumAll = table.Column<decimal>(type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Writeofs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Writeofs_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "WriteofGoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WriteofId = table.Column<int>(type: "int", nullable: false),
                    GoodId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<double>(type: "double", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WriteofGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WriteofGoods_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WriteofGoods_Writeofs_WriteofId",
                        column: x => x.WriteofId,
                        principalTable: "Writeofs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_WriteofGoods_GoodId",
                table: "WriteofGoods",
                column: "GoodId");

            migrationBuilder.CreateIndex(
                name: "IX_WriteofGoods_WriteofId",
                table: "WriteofGoods",
                column: "WriteofId");

            migrationBuilder.CreateIndex(
                name: "IX_Writeofs_ShopId",
                table: "Writeofs",
                column: "ShopId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WriteofGoods");

            migrationBuilder.DropTable(
                name: "Writeofs");
        }
    }
}
