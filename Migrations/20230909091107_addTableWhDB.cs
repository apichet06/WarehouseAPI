using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse_API.Migrations
{
    /// <inheritdoc />
    public partial class addTableWhDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Divisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DV_ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DV_Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divisions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncomingStocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncomingStockID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ProductID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    QtyReceived = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingStocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutgoingStocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutgoingStockID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ProductID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    QTYWithdrawn = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    WithdrawalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WithdrawnBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingStocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    P_ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    P_Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DV_ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProductDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PImages = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    QtyMin = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    QtyInStock = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ReceiveAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    lastAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DV_ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    P_ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Divisions");

            migrationBuilder.DropTable(
                name: "IncomingStocks");

            migrationBuilder.DropTable(
                name: "OutgoingStocks");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
