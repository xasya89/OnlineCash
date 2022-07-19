using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseBuyer.Migrations
{
    public partial class AlterBuyer_Add_SumDiscountInBuy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SumDiscountInBuy",
                table: "Buyers",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SumDiscountInBuy",
                table: "Buyers");
        }
    }
}
