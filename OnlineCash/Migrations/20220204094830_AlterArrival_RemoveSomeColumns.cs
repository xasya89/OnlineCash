using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AlterArrival_RemoveSomeColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountAll",
                table: "Arrivals");

            migrationBuilder.DropColumn(
                name: "PriceAll",
                table: "Arrivals");

            migrationBuilder.RenameColumn(
                name: "SumArrivals",
                table: "Arrivals",
                newName: "SumArrival");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SumArrival",
                table: "Arrivals",
                newName: "SumArrivals");

            migrationBuilder.AddColumn<double>(
                name: "CountAll",
                table: "Arrivals",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceAll",
                table: "Arrivals",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
