using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DIYshopAPI.Migrations.Promotion
{
    /// <inheritdoc />
    public partial class addPromotionContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartPromotion = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndPromotion = table.Column<DateTime>(type: "datetime", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Promotions");
        }
    }
}
