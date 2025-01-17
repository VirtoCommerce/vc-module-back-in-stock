using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.BackInStock.Data.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddBackInStockSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BackInStockSubscription",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    StoreId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    MemberId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackInStockSubscription", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BackInStockSubscription_MemberId",
                table: "BackInStockSubscription",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_BackInStockSubscription_ProductId_IsActive",
                table: "BackInStockSubscription",
                columns: new[] { "ProductId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_BackInStockSubscription_UserId_ProductId_StoreId_IsActive",
                table: "BackInStockSubscription",
                columns: new[] { "UserId", "ProductId", "StoreId", "IsActive" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackInStockSubscription");
        }
    }
}
