using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCashRmk.Migrations
{
    public partial class Alter_ArrivalGood_AddPriceSell : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PriceSell",
                table: "ArrivalGoods",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceSell",
                table: "ArrivalGoods");
        }
    }
}
