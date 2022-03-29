using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AddReportAfterStocktck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RevaluationArrival",
                table: "ReportsAfterStocktaking",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RevaluationWriteOf",
                table: "ReportsAfterStocktaking",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RevaluationArrival",
                table: "ReportsAfterStocktaking");

            migrationBuilder.DropColumn(
                name: "RevaluationWriteOf",
                table: "ReportsAfterStocktaking");
        }
    }
}
