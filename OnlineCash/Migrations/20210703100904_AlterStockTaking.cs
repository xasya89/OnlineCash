using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class AlterStockTaking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropForeignKey(
                name: "FK_StocktakingGoods_Stocktakings_StocktakingId",
                table: "StocktakingGoods");
            
            migrationBuilder.RenameColumn(
                name: "StocktakingId",
                table: "StocktakingGoods",
                newName: "StockTakingGroupId");
            
            migrationBuilder.RenameIndex(
                name: "IX_StocktakingGoods_StocktakingId",
                table: "StocktakingGoods",
                newName: "IX_StocktakingGoods_StockTakingGroupId");
            
            migrationBuilder.CreateTable(
                name: "StockTakingGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    StocktakingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTakingGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTakingGroups_Stocktakings_StocktakingId",
                        column: x => x.StocktakingId,
                        principalTable: "Stocktakings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_StockTakingGroups_StocktakingId",
                table: "StockTakingGroups",
                column: "StocktakingId");

            migrationBuilder.AddForeignKey(
                name: "FK_StocktakingGoods_StockTakingGroups_StockTakingGroupId",
                table: "StocktakingGoods",
                column: "StockTakingGroupId",
                principalTable: "StockTakingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StocktakingGoods_StockTakingGroups_StockTakingGroupId",
                table: "StocktakingGoods");

            migrationBuilder.DropTable(
                name: "StockTakingGroups");

            migrationBuilder.RenameColumn(
                name: "StockTakingGroupId",
                table: "StocktakingGoods",
                newName: "StocktakingId");

            migrationBuilder.RenameIndex(
                name: "IX_StocktakingGoods_StockTakingGroupId",
                table: "StocktakingGoods",
                newName: "IX_StocktakingGoods_StocktakingId");

            migrationBuilder.AddForeignKey(
                name: "FK_StocktakingGoods_Stocktakings_StocktakingId",
                table: "StocktakingGoods",
                column: "StocktakingId",
                principalTable: "Stocktakings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
