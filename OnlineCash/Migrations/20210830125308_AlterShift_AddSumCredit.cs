using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AlterShift_AddSumCredit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SumCreditDelivery",
                table: "Shifts",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SumCreditRepayment",
                table: "Shifts",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SumElectron",
                table: "Shifts",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SumNoElectron",
                table: "Shifts",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SumCreditDelivery",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "SumCreditRepayment",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "SumElectron",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "SumNoElectron",
                table: "Shifts");
        }
    }
}
