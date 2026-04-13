using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Construction.Service.Migrations
{
    /// <inheritdoc />
    public partial class dbUpdate_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Works",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Works",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Works");
        }
    }
}
