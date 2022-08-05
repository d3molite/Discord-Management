using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class feedbackcommand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeedbackConfig",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AddReactions = table.Column<bool>(type: "INTEGER", nullable: false),
                    FeedbackChannelID = table.Column<ulong>(type: "INTEGER", nullable: false),
                    BotConfigID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackConfig", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FeedbackConfig_Configs_BotConfigID",
                        column: x => x.BotConfigID,
                        principalTable: "Configs",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackConfig_BotConfigID",
                table: "FeedbackConfig",
                column: "BotConfigID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedbackConfig");
        }
    }
}
