using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AddRevaluation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Revaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Create = table.Column<DateTime>(type: "Date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revaluations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "RevaluationGoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RevaluationId = table.Column<int>(type: "int", nullable: false),
                    GoodId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PriceOld = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PriceNew = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevaluationGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RevaluationGoods_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RevaluationGoods_Revaluations_RevaluationId",
                        column: x => x.RevaluationId,
                        principalTable: "Revaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_RevaluationGoods_GoodId",
                table: "RevaluationGoods",
                column: "GoodId");

            migrationBuilder.CreateIndex(
                name: "IX_RevaluationGoods_RevaluationId",
                table: "RevaluationGoods",
                column: "RevaluationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RevaluationGoods");

            migrationBuilder.DropTable(
                name: "Revaluations");
        }
    }
}
