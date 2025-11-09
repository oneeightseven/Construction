using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Construction.Service.Migrations
{
    /// <inheritdoc />
    public partial class AlterColumsShoppingMall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "ShoppingMalls",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "ShoppingMalls",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "ShoppingMalls");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "ShoppingMalls");
        }
    }
}
