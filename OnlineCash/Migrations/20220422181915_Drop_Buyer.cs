using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class Drop_Buyer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckSells_Buyers_BuyerId",
                table: "CheckSells");

            migrationBuilder.DropTable(
                name: "PersonalDiscounts");

            migrationBuilder.DropTable(
                name: "Buyers");

            migrationBuilder.DropTable(
                name: "DiscountCards");

            migrationBuilder.DropIndex(
                name: "IX_CheckSells_BuyerId",
                table: "CheckSells");

            migrationBuilder.AddColumn<string>(
                name: "BuyerName",
                table: "CheckSells",
                type: "longtext",
                nullable: true,
                collation: "utf8_general_ci")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "BuyerPhone",
                table: "CheckSells",
                type: "longtext",
                nullable: true,
                collation: "utf8_general_ci")
                .Annotation("MySql:CharSet", "utf8");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerName",
                table: "CheckSells");

            migrationBuilder.DropColumn(
                name: "BuyerPhone",
                table: "CheckSells");

            migrationBuilder.CreateTable(
                name: "DiscountCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Num = table.Column<string>(type: "longtext", nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    isFree = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCards", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "Buyers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Birthday = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DiscountCardId = table.Column<int>(type: "int", nullable: false),
                    DiscountPercant = table.Column<int>(type: "int", nullable: true),
                    DiscountSum = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Phone = table.Column<string>(type: "longtext", nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    SumBuy = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Uuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    isBlock = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buyers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buyers_DiscountCards_DiscountCardId",
                        column: x => x.DiscountCardId,
                        principalTable: "DiscountCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateTable(
                name: "PersonalDiscounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BuyerId = table.Column<int>(type: "int", nullable: false),
                    DateAccept = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DiscountPercent = table.Column<int>(type: "int", nullable: true),
                    DiscountSum = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Note = table.Column<string>(type: "longtext", nullable: true, collation: "utf8_general_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Uuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalDiscounts_Buyers_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Buyers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_CheckSells_BuyerId",
                table: "CheckSells",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Buyers_DiscountCardId",
                table: "Buyers",
                column: "DiscountCardId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalDiscounts_BuyerId",
                table: "PersonalDiscounts",
                column: "BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckSells_Buyers_BuyerId",
                table: "CheckSells",
                column: "BuyerId",
                principalTable: "Buyers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
