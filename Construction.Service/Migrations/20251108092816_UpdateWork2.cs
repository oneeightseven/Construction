using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Construction.Service.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWork2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Works",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Works_EmployeeId",
                table: "Works",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Employees_EmployeeId",
                table: "Works",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Works_Employees_EmployeeId",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Works_EmployeeId",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Works");
        }
    }
}
