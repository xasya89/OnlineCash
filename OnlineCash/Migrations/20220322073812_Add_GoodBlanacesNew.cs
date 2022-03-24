using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class Add_GoodBlanacesNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoodCountBalances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Period = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    GoodId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<decimal>(type: "decimal(10,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodCountBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodCountBalances_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "GoodCountDocHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    GoodId = table.Column<int>(type: "int", nullable: false),
                    DocId = table.Column<int>(type: "int", nullable: false),
                    TypeDoc = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<decimal>(type: "decimal(10,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodCountDocHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodCountDocHistories_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_GoodCountBalances_GoodId",
                table: "GoodCountBalances",
                column: "GoodId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodCountBalances_Period_GoodId",
                table: "GoodCountBalances",
                columns: new[] { "Period", "GoodId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoodCountDocHistories_GoodId",
                table: "GoodCountDocHistories",
                column: "GoodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoodCountBalances");

            migrationBuilder.DropTable(
                name: "GoodCountDocHistories");
        }
    }
}
