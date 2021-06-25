using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RMK.Migrations
{
    public partial class AlterChecSell_AddRelationToShift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreate",
                table: "CheckSells",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ShiftId",
                table: "CheckSells",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CheckSells_ShiftId",
                table: "CheckSells",
                column: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckSells_Shifts_ShiftId",
                table: "CheckSells",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckSells_Shifts_ShiftId",
                table: "CheckSells");

            migrationBuilder.DropIndex(
                name: "IX_CheckSells_ShiftId",
                table: "CheckSells");

            migrationBuilder.DropColumn(
                name: "DateCreate",
                table: "CheckSells");

            migrationBuilder.DropColumn(
                name: "ShiftId",
                table: "CheckSells");
        }
    }
}
