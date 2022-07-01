using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class messagereactionswitch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageReactionConfig_Configs_BotConfigID",
                table: "MessageReactionConfig");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageReactionConfig_Emojis_ReactionEmojiID",
                table: "MessageReactionConfig");

            migrationBuilder.DropIndex(
                name: "IX_MessageReactionConfig_BotConfigID",
                table: "MessageReactionConfig");

            migrationBuilder.DropIndex(
                name: "IX_MessageReactionConfig_ReactionEmojiID",
                table: "MessageReactionConfig");

            migrationBuilder.DropColumn(
                name: "BotConfigID",
                table: "MessageReactionConfig");

            migrationBuilder.DropColumn(
                name: "EmojiOnly",
                table: "MessageReactionConfig");

            migrationBuilder.DropColumn(
                name: "ReactionChance",
                table: "MessageReactionConfig");

            migrationBuilder.DropColumn(
                name: "ReactionEmojiID",
                table: "MessageReactionConfig");

            migrationBuilder.DropColumn(
                name: "ReactionMessage",
                table: "MessageReactionConfig");

            migrationBuilder.DropColumn(
                name: "ReactionTrigger",
                table: "MessageReactionConfig");

            migrationBuilder.AddColumn<int>(
                name: "ReactionConfigID",
                table: "Configs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MessageReaction",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReactionEmojiID = table.Column<int>(type: "INTEGER", nullable: true),
                    ReactionMessage = table.Column<string>(type: "TEXT", nullable: true),
                    EmojiOnly = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReactionTrigger = table.Column<string>(type: "TEXT", nullable: false),
                    ReactionChance = table.Column<int>(type: "INTEGER", nullable: false),
                    MessageReactionConfigID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageReaction", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MessageReaction_Emojis_ReactionEmojiID",
                        column: x => x.ReactionEmojiID,
                        principalTable: "Emojis",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_MessageReaction_MessageReactionConfig_MessageReactionConfigID",
                        column: x => x.MessageReactionConfigID,
                        principalTable: "MessageReactionConfig",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configs_ReactionConfigID",
                table: "Configs",
                column: "ReactionConfigID");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReaction_MessageReactionConfigID",
                table: "MessageReaction",
                column: "MessageReactionConfigID");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReaction_ReactionEmojiID",
                table: "MessageReaction",
                column: "ReactionEmojiID");

            migrationBuilder.AddForeignKey(
                name: "FK_Configs_MessageReactionConfig_ReactionConfigID",
                table: "Configs",
                column: "ReactionConfigID",
                principalTable: "MessageReactionConfig",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Configs_MessageReactionConfig_ReactionConfigID",
                table: "Configs");

            migrationBuilder.DropTable(
                name: "MessageReaction");

            migrationBuilder.DropIndex(
                name: "IX_Configs_ReactionConfigID",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "ReactionConfigID",
                table: "Configs");

            migrationBuilder.AddColumn<int>(
                name: "BotConfigID",
                table: "MessageReactionConfig",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmojiOnly",
                table: "MessageReactionConfig",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReactionChance",
                table: "MessageReactionConfig",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReactionEmojiID",
                table: "MessageReactionConfig",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReactionMessage",
                table: "MessageReactionConfig",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReactionTrigger",
                table: "MessageReactionConfig",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReactionConfig_BotConfigID",
                table: "MessageReactionConfig",
                column: "BotConfigID");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReactionConfig_ReactionEmojiID",
                table: "MessageReactionConfig",
                column: "ReactionEmojiID");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageReactionConfig_Configs_BotConfigID",
                table: "MessageReactionConfig",
                column: "BotConfigID",
                principalTable: "Configs",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageReactionConfig_Emojis_ReactionEmojiID",
                table: "MessageReactionConfig",
                column: "ReactionEmojiID",
                principalTable: "Emojis",
                principalColumn: "ID");
        }
    }
}
