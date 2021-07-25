using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AlterTable_Stocktaking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Count",
                table: "Stocktakings",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CountFact",
                table: "Stocktakings",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "Sum",
                table: "Stocktakings",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SumFact",
                table: "Stocktakings",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Stocktakings");

            migrationBuilder.DropColumn(
                name: "CountFact",
                table: "Stocktakings");

            migrationBuilder.DropColumn(
                name: "Sum",
                table: "Stocktakings");

            migrationBuilder.DropColumn(
                name: "SumFact",
                table: "Stocktakings");
        }
    }
}
