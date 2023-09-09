using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse_API.Migrations
{
    /// <inheritdoc />
    public partial class updateproductQtyMinStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QtyMin",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "QtyMinStock",
                table: "Products",
                type: "int",
                maxLength: 10,
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QtyMinStock",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "QtyMin",
                table: "Products",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }
    }
}
