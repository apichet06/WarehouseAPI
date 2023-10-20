using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse_API.Migrations
{
    /// <inheritdoc />
    public partial class addinventoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutgoingStocks");

            migrationBuilder.CreateTable(
                name: "InventoryRequests",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TransactionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequesterUserId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DivisionId = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    ApproveBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    AppvDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryRequests", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "picking_GoodsDetails",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutgoingStockID = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    ProductID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    QTYWithdrawn = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WithdrawnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WithdrawnBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ApproveBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    AppvDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    IsApproved = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_picking_GoodsDetails", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryRequests");

            migrationBuilder.DropTable(
                name: "picking_GoodsDetails");

            migrationBuilder.CreateTable(
                name: "OutgoingStocks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApproveBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    AppvDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    OutgoingStockID = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    ProductID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    QTYWithdrawn = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WithdrawnBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    WithdrawnDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingStocks", x => x.ID);
                });
        }
    }
}
