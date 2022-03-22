using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AlterRevaluation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Revaluations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SumNew",
                table: "Revaluations",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SumOld",
                table: "Revaluations",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                table: "Revaluations",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Revaluations");

            migrationBuilder.DropColumn(
                name: "SumNew",
                table: "Revaluations");

            migrationBuilder.DropColumn(
                name: "SumOld",
                table: "Revaluations");

            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "Revaluations");
        }
    }
}
