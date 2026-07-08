using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_System.Migrations
{
    /// <inheritdoc />
    public partial class RenameOrderProductToOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "orderProductId",
                table: "OrderItem",
                newName: "orderItemId");

            migrationBuilder.AddColumn<decimal>(
                name: "unitPrice",
                table: "OrderItem",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "unitPrice",
                table: "OrderItem");

            migrationBuilder.RenameColumn(
                name: "orderItemId",
                table: "OrderItem",
                newName: "orderProductId");
        }
    }
}
