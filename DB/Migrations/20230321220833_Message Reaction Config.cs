using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class MessageReactionConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MessageReactionConfigId",
                table: "GuildConfigs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Emojis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmojiString = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emojis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageReactionConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageReactionConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageReactionItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReactionEmojiId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReactionMessage = table.Column<string>(type: "TEXT", nullable: true),
                    EmojiOnly = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReactionTrigger = table.Column<string>(type: "TEXT", nullable: false),
                    ReactionChance = table.Column<int>(type: "INTEGER", nullable: false),
                    MessageReactionConfigId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageReactionItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageReactionItem_Emojis_ReactionEmojiId",
                        column: x => x.ReactionEmojiId,
                        principalTable: "Emojis",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MessageReactionItem_MessageReactionConfig_MessageReactionConfigId",
                        column: x => x.MessageReactionConfigId,
                        principalTable: "MessageReactionConfig",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildConfigs_MessageReactionConfigId",
                table: "GuildConfigs",
                column: "MessageReactionConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReactionItem_MessageReactionConfigId",
                table: "MessageReactionItem",
                column: "MessageReactionConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReactionItem_ReactionEmojiId",
                table: "MessageReactionItem",
                column: "ReactionEmojiId");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildConfigs_MessageReactionConfig_MessageReactionConfigId",
                table: "GuildConfigs",
                column: "MessageReactionConfigId",
                principalTable: "MessageReactionConfig",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildConfigs_MessageReactionConfig_MessageReactionConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropTable(
                name: "MessageReactionItem");

            migrationBuilder.DropTable(
                name: "Emojis");

            migrationBuilder.DropTable(
                name: "MessageReactionConfig");

            migrationBuilder.DropIndex(
                name: "IX_GuildConfigs_MessageReactionConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropColumn(
                name: "MessageReactionConfigId",
                table: "GuildConfigs");
        }
    }
}
