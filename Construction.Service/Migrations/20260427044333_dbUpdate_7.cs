using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Construction.Service.Migrations
{
    /// <inheritdoc />
    public partial class dbUpdate_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkId",
                table: "StoredFiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StoredFiles_WorkId",
                table: "StoredFiles",
                column: "WorkId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoredFiles_Works_WorkId",
                table: "StoredFiles",
                column: "WorkId",
                principalTable: "Works",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoredFiles_Works_WorkId",
                table: "StoredFiles");

            migrationBuilder.DropIndex(
                name: "IX_StoredFiles_WorkId",
                table: "StoredFiles");

            migrationBuilder.DropColumn(
                name: "WorkId",
                table: "StoredFiles");
        }
    }
}
