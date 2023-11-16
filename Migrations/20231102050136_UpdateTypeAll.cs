using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTypeAll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_picking_GoodsDetails",
                table: "picking_GoodsDetails");

            migrationBuilder.RenameTable(
                name: "picking_GoodsDetails",
                newName: "Picking_GoodsDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppvDate",
                table: "Picking_GoodsDetails",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppvDate",
                table: "InventoryRequests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Picking_GoodsDetails",
                table: "Picking_GoodsDetails",
                column: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Picking_GoodsDetails",
                table: "Picking_GoodsDetails");

            migrationBuilder.RenameTable(
                name: "Picking_GoodsDetails",
                newName: "picking_GoodsDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppvDate",
                table: "picking_GoodsDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppvDate",
                table: "InventoryRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_picking_GoodsDetails",
                table: "picking_GoodsDetails",
                column: "ID");
        }
    }
}
