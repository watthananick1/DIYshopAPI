using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DIYshopAPI.Migrations.Order
{
    /// <inheritdoc />
    public partial class addOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total_Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Customer_Id = table.Column<int>(type: "int", nullable: true),
                    Promotion_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
