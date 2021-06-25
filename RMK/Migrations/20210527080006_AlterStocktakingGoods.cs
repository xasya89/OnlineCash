using Microsoft.EntityFrameworkCore.Migrations;

namespace RMK.Migrations
{
    public partial class AlterStocktakingGoods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "idGood",
                table: "StocktakingGoods",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "idGood",
                table: "StocktakingGoods");
        }
    }
}
