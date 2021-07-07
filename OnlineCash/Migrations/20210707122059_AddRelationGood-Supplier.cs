using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AddRelationGoodSupplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "Goods",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goods_SupplierId",
                table: "Goods",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Goods_Suppliers_SupplierId",
                table: "Goods",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goods_Suppliers_SupplierId",
                table: "Goods");

            migrationBuilder.DropIndex(
                name: "IX_Goods_SupplierId",
                table: "Goods");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Goods");
        }
    }
}
