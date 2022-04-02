using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineCash.Migrations
{
    public partial class Add_GoodCountBalaceCurrent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoodCountBalanceCurrents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GoodId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<decimal>(type: "decimal(10,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodCountBalanceCurrents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodCountBalanceCurrents_Goods_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Goods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("Relational:Collation", "utf8_general_ci");
            migrationBuilder.Sql(@"INSERT INTO goodcountbalancecurrents (GoodId, Count) 
SELECT g.id,ifnull(b.Count,0) FROM goods g LEFT JOIN goodbalances b ON g.id=b.GoodId;");
            migrationBuilder.Sql(@$"INSERT INTO goodcountbalances (Period, GoodId, Count) 
SELECT STR_TO_DATE('{DateTime.Now.ToString("01.MM.yyyy")}','%d.%m.%Y'), g.id, IFNULL(c.Count,0) FROM Goods g LEFT JOIN goodcountbalancecurrents c ON g.id=c.GoodId;");
            migrationBuilder.Sql(@$"INSERT INTO goodcountbalances (Period, GoodId, Count) 
SELECT STR_TO_DATE('{DateTime.Now.AddMonths(1).ToString("01.MM.yyyy")}','%d.%m.%Y'), g.id, IFNULL(c.Count,0) FROM Goods g LEFT JOIN goodcountbalancecurrents c ON g.id=c.GoodId;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
