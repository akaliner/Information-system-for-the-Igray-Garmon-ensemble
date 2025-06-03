using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _1.Migrations
{
    /// <inheritdoc />
    public partial class _3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AudApplicants");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInfo",
                table: "Schedules",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuditionRequestId",
                table: "Auditions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Auditions_AuditionRequestId",
                table: "Auditions",
                column: "AuditionRequestId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Auditions_AuditionRequests_AuditionRequestId",
                table: "Auditions",
                column: "AuditionRequestId",
                principalTable: "AuditionRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auditions_AuditionRequests_AuditionRequestId",
                table: "Auditions");

            migrationBuilder.DropIndex(
                name: "IX_Auditions_AuditionRequestId",
                table: "Auditions");

            migrationBuilder.DropColumn(
                name: "AdditionalInfo",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "AuditionRequestId",
                table: "Auditions");

            migrationBuilder.CreateTable(
                name: "AudApplicants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AuditionId = table.Column<int>(type: "INTEGER", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudApplicants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AudApplicants_Auditions_AuditionId",
                        column: x => x.AuditionId,
                        principalTable: "Auditions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AudApplicants_AuditionId",
                table: "AudApplicants",
                column: "AuditionId");
        }
    }
}
