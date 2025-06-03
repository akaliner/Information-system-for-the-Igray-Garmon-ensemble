using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _1.Migrations
{
    /// <inheritdoc />
    public partial class respempl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResponsibleEmployeeId",
                table: "Auditions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Auditions_ResponsibleEmployeeId",
                table: "Auditions",
                column: "ResponsibleEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auditions_Users_ResponsibleEmployeeId",
                table: "Auditions",
                column: "ResponsibleEmployeeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auditions_Users_ResponsibleEmployeeId",
                table: "Auditions");

            migrationBuilder.DropIndex(
                name: "IX_Auditions_ResponsibleEmployeeId",
                table: "Auditions");

            migrationBuilder.DropColumn(
                name: "ResponsibleEmployeeId",
                table: "Auditions");
        }
    }
}
