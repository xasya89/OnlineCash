using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class Alter_ReportsAfterStocktaking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StocktakingId",
                table: "ReportsAfterStocktaking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReportsAfterStocktaking_StocktakingId",
                table: "ReportsAfterStocktaking",
                column: "StocktakingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportsAfterStocktaking_Stocktakings_StocktakingId",
                table: "ReportsAfterStocktaking",
                column: "StocktakingId",
                principalTable: "Stocktakings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportsAfterStocktaking_Stocktakings_StocktakingId",
                table: "ReportsAfterStocktaking");

            migrationBuilder.DropIndex(
                name: "IX_ReportsAfterStocktaking_StocktakingId",
                table: "ReportsAfterStocktaking");

            migrationBuilder.DropColumn(
                name: "StocktakingId",
                table: "ReportsAfterStocktaking");
        }
    }
}
