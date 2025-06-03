using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _1.Migrations
{
    /// <inheritdoc />
    public partial class Schedule1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Roles_RoleId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_RoleId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Schedules");

            migrationBuilder.CreateTable(
                name: "ScheduleRoles",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleRoles", x => new { x.ScheduleId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ScheduleRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleRoles_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleRoles_RoleId",
                table: "ScheduleRoles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduleRoles");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Schedules",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_RoleId",
                table: "Schedules",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Roles_RoleId",
                table: "Schedules",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");
        }
    }
}
