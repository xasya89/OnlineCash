using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AddTableShifts_add_Checks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckGood_CheckSell_CheckSellId",
                table: "CheckGood");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckGood_Goods_GoodId",
                table: "CheckGood");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckSell_Shift_ShiftId",
                table: "CheckSell");

            migrationBuilder.DropForeignKey(
                name: "FK_Shift_Cashiers_CashierId",
                table: "Shift");

            migrationBuilder.DropForeignKey(
                name: "FK_Shift_Shops_ShopId",
                table: "Shift");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shift",
                table: "Shift");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckSell",
                table: "CheckSell");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckGood",
                table: "CheckGood");

            migrationBuilder.RenameTable(
                name: "Shift",
                newName: "Shifts");

            migrationBuilder.RenameTable(
                name: "CheckSell",
                newName: "CheckSells");

            migrationBuilder.RenameTable(
                name: "CheckGood",
                newName: "CheckGoods");

            migrationBuilder.RenameIndex(
                name: "IX_Shift_ShopId",
                table: "Shifts",
                newName: "IX_Shifts_ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_Shift_CashierId",
                table: "Shifts",
                newName: "IX_Shifts_CashierId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckSell_ShiftId",
                table: "CheckSells",
                newName: "IX_CheckSells_ShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckGood_GoodId",
                table: "CheckGoods",
                newName: "IX_CheckGoods_GoodId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckGood_CheckSellId",
                table: "CheckGoods",
                newName: "IX_CheckGoods_CheckSellId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shifts",
                table: "Shifts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckSells",
                table: "CheckSells",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckGoods",
                table: "CheckGoods",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckGoods_CheckSells_CheckSellId",
                table: "CheckGoods",
                column: "CheckSellId",
                principalTable: "CheckSells",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckGoods_Goods_GoodId",
                table: "CheckGoods",
                column: "GoodId",
                principalTable: "Goods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckSells_Shifts_ShiftId",
                table: "CheckSells",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_CheckGoods_CheckSells_CheckSellId",
                table: "CheckGoods");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckGoods_Goods_GoodId",
                table: "CheckGoods");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckSells_Shifts_ShiftId",
                table: "CheckSells");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Cashiers_CashierId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Shops_ShopId",
                table: "Shifts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shifts",
                table: "Shifts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckSells",
                table: "CheckSells");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckGoods",
                table: "CheckGoods");

            migrationBuilder.RenameTable(
                name: "Shifts",
                newName: "Shift");

            migrationBuilder.RenameTable(
                name: "CheckSells",
                newName: "CheckSell");

            migrationBuilder.RenameTable(
                name: "CheckGoods",
                newName: "CheckGood");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_ShopId",
                table: "Shift",
                newName: "IX_Shift_ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_CashierId",
                table: "Shift",
                newName: "IX_Shift_CashierId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckSells_ShiftId",
                table: "CheckSell",
                newName: "IX_CheckSell_ShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckGoods_GoodId",
                table: "CheckGood",
                newName: "IX_CheckGood_GoodId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckGoods_CheckSellId",
                table: "CheckGood",
                newName: "IX_CheckGood_CheckSellId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shift",
                table: "Shift",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckSell",
                table: "CheckSell",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckGood",
                table: "CheckGood",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckGood_CheckSell_CheckSellId",
                table: "CheckGood",
                column: "CheckSellId",
                principalTable: "CheckSell",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckGood_Goods_GoodId",
                table: "CheckGood",
                column: "GoodId",
                principalTable: "Goods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckSell_Shift_ShiftId",
                table: "CheckSell",
                column: "ShiftId",
                principalTable: "Shift",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
