using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class messagereactionconfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MessageReactionConfig",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReactionEmojiID = table.Column<int>(type: "INTEGER", nullable: true),
                    ReactionMessage = table.Column<string>(type: "TEXT", nullable: true),
                    EmojiOnly = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReactionTrigger = table.Column<string>(type: "TEXT", nullable: false),
                    BotConfigID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageReactionConfig", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MessageReactionConfig_Configs_BotConfigID",
                        column: x => x.BotConfigID,
                        principalTable: "Configs",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_MessageReactionConfig_Emojis_ReactionEmojiID",
                        column: x => x.ReactionEmojiID,
                        principalTable: "Emojis",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageReactionConfig_BotConfigID",
                table: "MessageReactionConfig",
                column: "BotConfigID");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReactionConfig_ReactionEmojiID",
                table: "MessageReactionConfig",
                column: "ReactionEmojiID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageReactionConfig");
        }
    }
}
