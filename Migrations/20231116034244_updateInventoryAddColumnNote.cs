using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse_API.Migrations
{
    /// <inheritdoc />
    public partial class updateInventoryAddColumnNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "InventoryRequests",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "InventoryRequests");
        }
    }
}
