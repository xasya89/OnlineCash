using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AddMoneyBalanceHistories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MoneyBalanceHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ShopId = table.Column<int>(type: "int", nullable: false),
                    DateBalance = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SumSale = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SumReturn = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SumIncome = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SumOutcome = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SumOther = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SumEnd = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyBalanceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoneyBalanceHistories_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyBalanceHistories_ShopId",
                table: "MoneyBalanceHistories",
                column: "ShopId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoneyBalanceHistories");
        }
    }
}
