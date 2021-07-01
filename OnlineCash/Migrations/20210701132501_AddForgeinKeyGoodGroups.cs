using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AddForgeinKeyGoodGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.DropForeignKey(
                name: "FK_Goods_GoodGroups_GoodGroupId",
                table: "Goods");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Goods");

            migrationBuilder.AlterColumn<int>(
                name: "GoodGroupId",
                table: "Goods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
            */
            migrationBuilder.AddForeignKey(
                name: "FK_Goods_GoodGroups_GoodGroupId",
                table: "Goods",
                column: "GoodGroupId",
                principalTable: "GoodGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goods_GoodGroups_GoodGroupId",
                table: "Goods");

            migrationBuilder.AlterColumn<int>(
                name: "GoodGroupId",
                table: "Goods",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Goods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Goods_GoodGroups_GoodGroupId",
                table: "Goods",
                column: "GoodGroupId",
                principalTable: "GoodGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
