using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class Alter_CheckPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Income",
                table: "CheckPayments");

            migrationBuilder.DropColumn(
                name: "Retturn",
                table: "CheckPayments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Income",
                table: "CheckPayments",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Retturn",
                table: "CheckPayments",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
