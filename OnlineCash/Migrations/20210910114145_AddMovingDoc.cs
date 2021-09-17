using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AddMovingDoc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MoveDocs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsSuccess = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DateMove = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ConsignerShopId = table.Column<int>(type: "int", nullable: false),
                    ConsigneeShopId = table.Column<int>(type: "int", nullable: false),
                    SumAllConsigner = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    SumAllConsignee = table.Column<decimal>(type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoveDocs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoveDocs_Shops_ConsigneeShopId",
                        column: x => x.ConsigneeShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoveDocs_Shops_ConsignerShopId",
                        column: x => x.ConsignerShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "MoveGoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MoveDocId = table.Column<int>(type: "int", nullable: false),
                    GoodId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<double>(type: "double", nullable: false),
                    PriceConsigner = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PriceConsignee = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoveGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoveGoods_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoveGoods_MoveDocs_MoveDocId",
                        column: x => x.MoveDocId,
                        principalTable: "MoveDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_MoveDocs_ConsigneeShopId",
                table: "MoveDocs",
                column: "ConsigneeShopId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveDocs_ConsignerShopId",
                table: "MoveDocs",
                column: "ConsignerShopId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveGoods_GoodId",
                table: "MoveGoods",
                column: "GoodId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveGoods_MoveDocId",
                table: "MoveGoods",
                column: "MoveDocId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoveGoods");

            migrationBuilder.DropTable(
                name: "MoveDocs");
        }
    }
}
