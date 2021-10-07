using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AlterShift_StopNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftSales_Goods_GoodId",
                table: "ShiftSales");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftSales_Shifts_ShiftId",
                table: "ShiftSales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShiftSales",
                table: "ShiftSales");

            migrationBuilder.RenameTable(
                name: "ShiftSales",
                newName: "ShiftSales");

            migrationBuilder.RenameIndex(
                name: "IX_ShiftSales_ShiftId",
                table: "ShiftSales",
                newName: "IX_ShiftSales_ShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_ShiftSales_GoodId",
                table: "ShiftSales",
                newName: "IX_ShiftSales_GoodId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShiftSales",
                table: "ShiftSales",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftSales_Goods_GoodId",
                table: "ShiftSales",
                column: "GoodId",
                principalTable: "Goods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftSales_Shifts_ShiftId",
                table: "ShiftSales",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftSales_Goods_GoodId",
                table: "ShiftSales");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftSales_Shifts_ShiftId",
                table: "ShiftSales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShiftSales",
                table: "ShiftSales");

            migrationBuilder.RenameTable(
                name: "ShiftSales",
                newName: "ShiftSales");

            migrationBuilder.RenameIndex(
                name: "IX_ShiftSales_ShiftId",
                table: "ShiftSales",
                newName: "IX_ShiftSale_ShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_ShiftSales_GoodId",
                table: "ShiftSales",
                newName: "IX_ShiftSale_GoodId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShiftSales",
                table: "ShiftSales",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftSales_Goods_GoodId",
                table: "ShiftSales",
                column: "GoodId",
                principalTable: "Goods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftSales_Shifts_ShiftId",
                table: "ShiftSale",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
