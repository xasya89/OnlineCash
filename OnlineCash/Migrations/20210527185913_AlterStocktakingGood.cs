using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AlterStocktakingGood : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CountDB",
                table: "StocktakingGoods",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "StocktakingGoods",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountDB",
                table: "StocktakingGoods");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "StocktakingGoods");
        }
    }
}
