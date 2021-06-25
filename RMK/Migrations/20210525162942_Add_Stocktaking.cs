using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RMK.Migrations
{
    public partial class Add_Stocktaking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stocktakings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Num = table.Column<int>(nullable: false),
                    Create = table.Column<DateTime>(nullable: false),
                    ShopId = table.Column<int>(nullable: false),
                    isComplite = table.Column<bool>(nullable: false),
                    isSending = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocktakings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StocktakingGoods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GoodUuid = table.Column<Guid>(nullable: false),
                    Count = table.Column<double>(nullable: false),
                    StocktakingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StocktakingGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StocktakingGoods_Stocktakings_StocktakingId",
                        column: x => x.StocktakingId,
                        principalTable: "Stocktakings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StocktakingGoods_StocktakingId",
                table: "StocktakingGoods",
                column: "StocktakingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StocktakingGoods");

            migrationBuilder.DropTable(
                name: "Stocktakings");
        }
    }
}
