using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class ALter_Arrival_ADD_DocStatus_GoodCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Arrivals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Count",
                table: "ArrivalGoods",
                type: "decimal(10,3)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");
            migrationBuilder.Sql("UPDATE arrivals SET Status=0 WHERE isSuccess=0");
            migrationBuilder.Sql("UPDATE arrivals SET Status=2 WHERE isSuccess=1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Arrivals");

            migrationBuilder.AlterColumn<double>(
                name: "Count",
                table: "ArrivalGoods",
                type: "double",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");
        }
    }
}
