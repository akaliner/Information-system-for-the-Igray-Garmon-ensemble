using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _1.Migrations
{
    /// <inheritdoc />
    public partial class cancellaud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Events_EventId",
                table: "Schedules");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Schedules",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Cancelled",
                table: "Auditions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Events_EventId",
                table: "Schedules",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Events_EventId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "Cancelled",
                table: "Auditions");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Schedules",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Events_EventId",
                table: "Schedules",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
