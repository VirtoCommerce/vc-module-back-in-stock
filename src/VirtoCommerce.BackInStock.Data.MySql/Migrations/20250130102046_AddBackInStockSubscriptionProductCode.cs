using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.BackInStock.Data.MySql.Migrations
{
    /// <inheritdoc />
    public partial class AddBackInStockSubscriptionProductCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "BackInStockSubscription",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "BackInStockSubscription");
        }
    }
}
