using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RMK.Migrations
{
    public partial class ChangeShift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shift_Cashiers_CashierId",
                table: "Shift");

            migrationBuilder.DropForeignKey(
                name: "FK_Shift_Shops_ShopId",
                table: "Shift");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shift",
                table: "Shift");

            migrationBuilder.RenameTable(
                name: "Shift",
                newName: "Shifts");

            migrationBuilder.RenameIndex(
                name: "IX_Shift_ShopId",
                table: "Shifts",
                newName: "IX_Shifts_ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_Shift_CashierId",
                table: "Shifts",
                newName: "IX_Shifts_CashierId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Stop",
                table: "Shifts",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shifts",
                table: "Shifts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Cashiers_CashierId",
                table: "Shifts",
                column: "CashierId",
                principalTable: "Cashiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Shops_ShopId",
                table: "Shifts",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Cashiers_CashierId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Shops_ShopId",
                table: "Shifts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shifts",
                table: "Shifts");

            migrationBuilder.RenameTable(
                name: "Shifts",
                newName: "Shift");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_ShopId",
                table: "Shift",
                newName: "IX_Shift_ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_CashierId",
                table: "Shift",
                newName: "IX_Shift_CashierId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Stop",
                table: "Shift",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shift",
                table: "Shift",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shift_Cashiers_CashierId",
                table: "Shift",
                column: "CashierId",
                principalTable: "Cashiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shift_Shops_ShopId",
                table: "Shift",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
