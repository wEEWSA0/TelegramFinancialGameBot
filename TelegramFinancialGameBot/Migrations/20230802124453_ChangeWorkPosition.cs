using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TelegramFinancialGameBot.Migrations
{
    /// <inheritdoc />
    public partial class ChangeWorkPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkPositions_WorkPositions_WorkPositionWorkId_WorkPosi~",
                table: "UserWorkPositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkPositions",
                table: "WorkPositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserWorkPositions",
                table: "UserWorkPositions");

            migrationBuilder.DropIndex(
                name: "IX_UserWorkPositions_WorkPositionWorkId_WorkPositionExpirience~",
                table: "UserWorkPositions");

            migrationBuilder.DropColumn(
                name: "WorkPositionWorkId",
                table: "UserWorkPositions");

            migrationBuilder.DropColumn(
                name: "WorkPositionExpirienceRequire",
                table: "UserWorkPositions");

            migrationBuilder.DropColumn(
                name: "WorkPositionId",
                table: "UserWorkPositions");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "WorkPositions",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "WorkPositionId",
                table: "UserWorkPositions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkPositions",
                table: "WorkPositions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserWorkPositions",
                table: "UserWorkPositions",
                columns: new[] { "UserId", "WorkPositionId" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkPositions_WorkId_ExpirienceRequire",
                table: "WorkPositions",
                columns: new[] { "WorkId", "ExpirienceRequire" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkPositions_WorkPositionId",
                table: "UserWorkPositions",
                column: "WorkPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkPositions_WorkPositions_WorkPositionId",
                table: "UserWorkPositions",
                column: "WorkPositionId",
                principalTable: "WorkPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkPositions_WorkPositions_WorkPositionId",
                table: "UserWorkPositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkPositions",
                table: "WorkPositions");

            migrationBuilder.DropIndex(
                name: "IX_WorkPositions_WorkId_ExpirienceRequire",
                table: "WorkPositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserWorkPositions",
                table: "UserWorkPositions");

            migrationBuilder.DropIndex(
                name: "IX_UserWorkPositions_WorkPositionId",
                table: "UserWorkPositions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "WorkPositions");

            migrationBuilder.DropColumn(
                name: "WorkPositionId",
                table: "UserWorkPositions");

            migrationBuilder.AddColumn<string>(
                name: "WorkPositionWorkId",
                table: "UserWorkPositions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<short>(
                name: "WorkPositionExpirienceRequire",
                table: "UserWorkPositions",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkPositions",
                table: "WorkPositions",
                columns: new[] { "WorkId", "ExpirienceRequire" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserWorkPositions",
                table: "UserWorkPositions",
                columns: new[] { "UserId", "WorkPositionWorkId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkPositions_WorkPositionWorkId_WorkPositionExpirience~",
                table: "UserWorkPositions",
                columns: new[] { "WorkPositionWorkId", "WorkPositionExpirienceRequire" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkPositions_WorkPositions_WorkPositionWorkId_WorkPosi~",
                table: "UserWorkPositions",
                columns: new[] { "WorkPositionWorkId", "WorkPositionExpirienceRequire" },
                principalTable: "WorkPositions",
                principalColumns: new[] { "WorkId", "ExpirienceRequire" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
