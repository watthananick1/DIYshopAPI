using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DIYshopAPI.Migrations.OrderItem
{
    /// <inheritdoc />
    public partial class addOrderItemDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order_Id = table.Column<int>(type: "int", nullable: false),
                    Product_id = table.Column<int>(type: "int", nullable: false),
                    N_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Item_Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Item_Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");
        }
    }
}
