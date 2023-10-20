using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse_API.Migrations
{
    /// <inheritdoc />
    public partial class updateIsapproveInventoryID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OutgoingStockID",
                table: "picking_GoodsDetails",
                newName: "Picking_goodsID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Picking_goodsID",
                table: "picking_GoodsDetails",
                newName: "OutgoingStockID");
        }
    }
}
