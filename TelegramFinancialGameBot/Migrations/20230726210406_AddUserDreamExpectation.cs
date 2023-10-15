using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TelegramFinancialGameBot.Migrations
{
    /// <inheritdoc />
    public partial class AddUserDreamExpectation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    ChatId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    isLink = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.ChatId);
                });

            migrationBuilder.CreateTable(
                name: "Buisnesses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RequireTime = table.Column<short>(type: "smallint", nullable: false),
                    VariableExpenses = table.Column<short>(type: "smallint", nullable: false),
                    CashIncome = table.Column<int>(type: "integer", nullable: false),
                    CashExpense = table.Column<int>(type: "integer", nullable: false),
                    Cost = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buisnesses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dreams",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StepCount = table.Column<short>(type: "smallint", nullable: false),
                    RequireTime = table.Column<short>(type: "smallint", nullable: false),
                    CashExpense = table.Column<int>(type: "integer", nullable: false),
                    Cost = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dreams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialDirectors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CashIncomePercent = table.Column<short>(type: "smallint", nullable: false),
                    CashExpense = table.Column<int>(type: "integer", nullable: false),
                    TimeIncome = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialDirectors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralDirectors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CashIncomePercent = table.Column<short>(type: "smallint", nullable: false),
                    CashExpense = table.Column<int>(type: "integer", nullable: false),
                    TimeIncome = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralDirectors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Knowledges",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LearningTime = table.Column<short>(type: "smallint", nullable: false),
                    RequireTime = table.Column<short>(type: "smallint", nullable: false),
                    CashExpense = table.Column<int>(type: "integer", nullable: false),
                    Cost = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Knowledges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ManagerStaffs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CashExpense = table.Column<int>(type: "integer", nullable: false),
                    TimeIncome = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerStaffs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RentCashIncome = table.Column<int>(type: "integer", nullable: false),
                    TimeExpense = table.Column<short>(type: "smallint", nullable: false),
                    CashExpense = table.Column<int>(type: "integer", nullable: false),
                    Cost = table.Column<int>(type: "integer", nullable: false),
                    EnergyCost = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegionalDirectors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CashExpense = table.Column<int>(type: "integer", nullable: false),
                    TimeIncome = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionalDirectors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Step = table.Column<short>(type: "smallint", nullable: false),
                    OwnerChatId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SetupCharacters",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Cash = table.Column<int>(type: "integer", nullable: false),
                    FreeTime = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetupCharacters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CashExpense = table.Column<int>(type: "integer", nullable: false),
                    TimeIncome = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountChatId = table.Column<long>(type: "bigint", nullable: false),
                    RoomId = table.Column<int>(type: "integer", nullable: false),
                    DreamId = table.Column<string>(type: "text", nullable: true),
                    CompleteDream = table.Column<bool>(type: "boolean", nullable: false),
                    Cash = table.Column<int>(type: "integer", nullable: false),
                    FreeTime = table.Column<int>(type: "integer", nullable: false),
                    CashIncome = table.Column<int>(type: "integer", nullable: false),
                    Energy = table.Column<short>(type: "smallint", nullable: false),
                    FinishedStep = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Accounts_AccountChatId",
                        column: x => x.AccountChatId,
                        principalTable: "Accounts",
                        principalColumn: "ChatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Dreams_DreamId",
                        column: x => x.DreamId,
                        principalTable: "Dreams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VictoryConditions",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "integer", nullable: false),
                    CashIncome = table.Column<int>(type: "integer", nullable: false),
                    RequireTime = table.Column<short>(type: "smallint", nullable: false),
                    TimeForPaymentsToBank = table.Column<short>(type: "smallint", nullable: false),
                    ShouldDreamBeCompleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VictoryConditions", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_VictoryConditions_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SetupCharacterCards",
                columns: table => new
                {
                    SetupCharacterId = table.Column<string>(type: "text", nullable: false),
                    Card = table.Column<string>(type: "text", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetupCharacterCards", x => new { x.SetupCharacterId, x.Card });
                    table.ForeignKey(
                        name: "FK_SetupCharacterCards_SetupCharacters_SetupCharacterId",
                        column: x => x.SetupCharacterId,
                        principalTable: "SetupCharacters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeForWork",
                columns: table => new
                {
                    KnowledgeId = table.Column<string>(type: "text", nullable: false),
                    WorkId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeForWork", x => new { x.KnowledgeId, x.WorkId });
                    table.ForeignKey(
                        name: "FK_KnowledgeForWork_Knowledges_KnowledgeId",
                        column: x => x.KnowledgeId,
                        principalTable: "Knowledges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KnowledgeForWork_Works_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkPositions",
                columns: table => new
                {
                    WorkId = table.Column<string>(type: "text", nullable: false),
                    ExpirienceRequire = table.Column<short>(type: "smallint", nullable: false),
                    Income = table.Column<int>(type: "integer", nullable: false),
                    RequireTime = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPositions", x => new { x.WorkId, x.ExpirienceRequire });
                    table.ForeignKey(
                        name: "FK_WorkPositions_Works_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBuisnesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    BuisnessId = table.Column<string>(type: "text", nullable: false),
                    BranchCount = table.Column<short>(type: "smallint", nullable: false),
                    OpenSteps = table.Column<short>(type: "smallint", nullable: false),
                    FinancialDirectorId = table.Column<string>(type: "text", nullable: true),
                    GeneralDirectorId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBuisnesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBuisnesses_Buisnesses_BuisnessId",
                        column: x => x.BuisnessId,
                        principalTable: "Buisnesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBuisnesses_FinancialDirectors_FinancialDirectorId",
                        column: x => x.FinancialDirectorId,
                        principalTable: "FinancialDirectors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserBuisnesses_GeneralDirectors_GeneralDirectorId",
                        column: x => x.GeneralDirectorId,
                        principalTable: "GeneralDirectors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserBuisnesses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDreamExpectations",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    DreamId = table.Column<string>(type: "text", nullable: false),
                    Steps = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDreamExpectations", x => new { x.UserId, x.DreamId });
                    table.ForeignKey(
                        name: "FK_UserDreamExpectations_Dreams_DreamId",
                        column: x => x.DreamId,
                        principalTable: "Dreams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDreamExpectations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserKnowledges",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    KnowledgeId = table.Column<string>(type: "text", nullable: false),
                    TimeToLearn = table.Column<short>(type: "smallint", nullable: false),
                    Experience = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserKnowledges", x => new { x.UserId, x.KnowledgeId });
                    table.ForeignKey(
                        name: "FK_UserKnowledges_Knowledges_KnowledgeId",
                        column: x => x.KnowledgeId,
                        principalTable: "Knowledges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserKnowledges_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProperties",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    PropertyId = table.Column<string>(type: "text", nullable: false),
                    UsesAsHome = table.Column<bool>(type: "boolean", nullable: false),
                    IsOwner = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProperties", x => new { x.UserId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_UserProperties_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProperties_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStaffs",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    StaffId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStaffs", x => new { x.UserId, x.StaffId });
                    table.ForeignKey(
                        name: "FK_UserStaffs_Staffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStaffs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWorkPositions",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    WorkPositionId = table.Column<string>(type: "text", nullable: false),
                    WorkPositionWorkId = table.Column<string>(type: "text", nullable: false),
                    WorkPositionExpirienceRequire = table.Column<short>(type: "smallint", nullable: false),
                    Experience = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWorkPositions", x => new { x.UserId, x.WorkPositionId });
                    table.ForeignKey(
                        name: "FK_UserWorkPositions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWorkPositions_WorkPositions_WorkPositionWorkId_WorkPosi~",
                        columns: x => new { x.WorkPositionWorkId, x.WorkPositionExpirienceRequire },
                        principalTable: "WorkPositions",
                        principalColumns: new[] { "WorkId", "ExpirienceRequire" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuisnessManagerStaffs",
                columns: table => new
                {
                    UserBuisnessId = table.Column<int>(type: "integer", nullable: false),
                    ManagerStaffId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuisnessManagerStaffs", x => new { x.UserBuisnessId, x.ManagerStaffId });
                    table.ForeignKey(
                        name: "FK_BuisnessManagerStaffs_ManagerStaffs_ManagerStaffId",
                        column: x => x.ManagerStaffId,
                        principalTable: "ManagerStaffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuisnessManagerStaffs_UserBuisnesses_UserBuisnessId",
                        column: x => x.UserBuisnessId,
                        principalTable: "UserBuisnesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuisnessRegionalDirectors",
                columns: table => new
                {
                    UserBuisnessId = table.Column<int>(type: "integer", nullable: false),
                    RegionalDirectorId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuisnessRegionalDirectors", x => new { x.UserBuisnessId, x.RegionalDirectorId });
                    table.ForeignKey(
                        name: "FK_BuisnessRegionalDirectors_RegionalDirectors_RegionalDirecto~",
                        column: x => x.RegionalDirectorId,
                        principalTable: "RegionalDirectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuisnessRegionalDirectors_UserBuisnesses_UserBuisnessId",
                        column: x => x.UserBuisnessId,
                        principalTable: "UserBuisnesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuisnessManagerStaffs_ManagerStaffId",
                table: "BuisnessManagerStaffs",
                column: "ManagerStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_BuisnessRegionalDirectors_RegionalDirectorId",
                table: "BuisnessRegionalDirectors",
                column: "RegionalDirectorId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeForWork_WorkId",
                table: "KnowledgeForWork",
                column: "WorkId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBuisnesses_BuisnessId",
                table: "UserBuisnesses",
                column: "BuisnessId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBuisnesses_FinancialDirectorId",
                table: "UserBuisnesses",
                column: "FinancialDirectorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBuisnesses_GeneralDirectorId",
                table: "UserBuisnesses",
                column: "GeneralDirectorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBuisnesses_UserId_BuisnessId",
                table: "UserBuisnesses",
                columns: new[] { "UserId", "BuisnessId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDreamExpectations_DreamId",
                table: "UserDreamExpectations",
                column: "DreamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserKnowledges_KnowledgeId",
                table: "UserKnowledges",
                column: "KnowledgeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProperties_PropertyId",
                table: "UserProperties",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AccountChatId_RoomId",
                table: "Users",
                columns: new[] { "AccountChatId", "RoomId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DreamId",
                table: "Users",
                column: "DreamId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoomId",
                table: "Users",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStaffs_StaffId",
                table: "UserStaffs",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkPositions_WorkPositionWorkId_WorkPositionExpirience~",
                table: "UserWorkPositions",
                columns: new[] { "WorkPositionWorkId", "WorkPositionExpirienceRequire" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuisnessManagerStaffs");

            migrationBuilder.DropTable(
                name: "BuisnessRegionalDirectors");

            migrationBuilder.DropTable(
                name: "KnowledgeForWork");

            migrationBuilder.DropTable(
                name: "SetupCharacterCards");

            migrationBuilder.DropTable(
                name: "UserDreamExpectations");

            migrationBuilder.DropTable(
                name: "UserKnowledges");

            migrationBuilder.DropTable(
                name: "UserProperties");

            migrationBuilder.DropTable(
                name: "UserStaffs");

            migrationBuilder.DropTable(
                name: "UserWorkPositions");

            migrationBuilder.DropTable(
                name: "VictoryConditions");

            migrationBuilder.DropTable(
                name: "ManagerStaffs");

            migrationBuilder.DropTable(
                name: "RegionalDirectors");

            migrationBuilder.DropTable(
                name: "UserBuisnesses");

            migrationBuilder.DropTable(
                name: "SetupCharacters");

            migrationBuilder.DropTable(
                name: "Knowledges");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Staffs");

            migrationBuilder.DropTable(
                name: "WorkPositions");

            migrationBuilder.DropTable(
                name: "Buisnesses");

            migrationBuilder.DropTable(
                name: "FinancialDirectors");

            migrationBuilder.DropTable(
                name: "GeneralDirectors");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Works");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Dreams");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
