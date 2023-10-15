using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramFinancialGameBot.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAccident : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAccidents",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    AccidentId = table.Column<string>(type: "text", nullable: false),
                    CurrentStep = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccidents", x => new { x.UserId, x.AccidentId });
                    table.ForeignKey(
                        name: "FK_UserAccidents_Accidents_AccidentId",
                        column: x => x.AccidentId,
                        principalTable: "Accidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAccidents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccidents_AccidentId",
                table: "UserAccidents",
                column: "AccidentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAccidents");
        }
    }
}
