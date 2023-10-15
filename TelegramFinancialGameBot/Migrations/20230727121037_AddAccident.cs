using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramFinancialGameBot.Migrations
{
    /// <inheritdoc />
    public partial class AddAccident : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accidents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CashExpense = table.Column<int>(type: "integer", nullable: false),
                    TimeExpense = table.Column<short>(type: "smallint", nullable: false),
                    EnergyCost = table.Column<short>(type: "smallint", nullable: false),
                    StepsDuration = table.Column<short>(type: "smallint", nullable: false),
                    Cost = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accidents", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accidents");
        }
    }
}
