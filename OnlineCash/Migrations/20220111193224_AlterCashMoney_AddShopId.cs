using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AlterCashMoney_AddShopId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "CashMoneys",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                collation: "utf8_general_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "CashMoneys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CashMoneys_ShopId",
                table: "CashMoneys",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_CashMoneys_Shops_ShopId",
                table: "CashMoneys",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashMoneys_Shops_ShopId",
                table: "CashMoneys");

            migrationBuilder.DropIndex(
                name: "IX_CashMoneys_ShopId",
                table: "CashMoneys");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "CashMoneys");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "CashMoneys",
                type: "longtext",
                nullable: true,
                collation: "utf8_general_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("Relational:Collation", "utf8_general_ci");
        }
    }
}
