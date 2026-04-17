using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Construction.Service.Migrations
{
    /// <inheritdoc />
    public partial class dbUpdate_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkId",
                table: "Accounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_WorkId",
                table: "Accounts",
                column: "WorkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Works_WorkId",
                table: "Accounts",
                column: "WorkId",
                principalTable: "Works",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Works_WorkId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_WorkId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "WorkId",
                table: "Accounts");
        }
    }
}
