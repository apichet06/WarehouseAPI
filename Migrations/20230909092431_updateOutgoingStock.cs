using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse_API.Migrations
{
    /// <inheritdoc />
    public partial class updateOutgoingStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WithdrawalDate",
                table: "OutgoingStocks",
                newName: "WithdrawnDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WithdrawnDate",
                table: "OutgoingStocks",
                newName: "WithdrawalDate");
        }
    }
}
