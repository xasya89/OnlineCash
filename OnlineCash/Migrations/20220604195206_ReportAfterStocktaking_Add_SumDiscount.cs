using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class ReportAfterStocktaking_Add_SumDiscount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SumDiscount",
                table: "ReportsAfterStocktaking",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SumDiscount",
                table: "ReportsAfterStocktaking");
        }
    }
}
