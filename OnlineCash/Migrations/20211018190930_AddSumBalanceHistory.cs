using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AddSumBalanceHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SumBalanceHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ShopId = table.Column<int>(type: "int", nullable: false),
                    DateHistory = table.Column<DateTime>(type: "date", nullable: false),
                    StartDaySum = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    StocktakingSum = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    ArrivalSum = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    WriteofSum = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    MoveSum = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    CashSalesSum = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    CashReturnSum = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    CashIncomeSum = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    CashOutcomeSum = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    EndDaySum = table.Column<decimal>(type: "decimal(11,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SumBalanceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SumBalanceHistories_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_SumBalanceHistories_ShopId",
                table: "SumBalanceHistories",
                column: "ShopId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SumBalanceHistories");
        }
    }
}
