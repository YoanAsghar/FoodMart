using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_Application.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsFirstPurchaseToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isFirstPurchase",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isFirstPurchase",
                table: "AspNetUsers");
        }
    }
}
