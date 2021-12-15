using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AlterBuyer_AddChecksellAndDiscountsum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "CheckSells",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountSum",
                table: "Buyers",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_CheckSells_BuyerId",
                table: "CheckSells",
                column: "BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckSells_Buyers_BuyerId",
                table: "CheckSells",
                column: "BuyerId",
                principalTable: "Buyers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckSells_Buyers_BuyerId",
                table: "CheckSells");

            migrationBuilder.DropIndex(
                name: "IX_CheckSells_BuyerId",
                table: "CheckSells");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "CheckSells");

            migrationBuilder.DropColumn(
                name: "DiscountSum",
                table: "Buyers");
        }
    }
}
