using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class Add_RepoertAfterStocktaking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportsAfterStocktaking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StartDate = table.Column<DateTime>(type: "Date", nullable: false),
                    StopDate = table.Column<DateTime>(type: "Date", nullable: false),
                    StocktakingPrependSum = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CashStartSum = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IncomeSum = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ElectronSum = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ArrivalSum = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    WriteOfSum = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CashEndSum = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    StocktakingFactSum = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportsAfterStocktaking", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportsAfterStocktaking");
        }
    }
}
