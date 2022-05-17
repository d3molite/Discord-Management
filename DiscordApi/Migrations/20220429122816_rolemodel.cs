using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class rolemodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AntiSpamConfig_Role_MutedRoleID",
                table: "AntiSpamConfig");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Roles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "Emojis",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmojiString = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emojis", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MessageID = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ReactionRoleConfig",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RelatedGuildId = table.Column<int>(type: "INTEGER", nullable: false),
                    RelatedMessageID = table.Column<int>(type: "INTEGER", nullable: false),
                    RelatedEmojiID = table.Column<int>(type: "INTEGER", nullable: false),
                    RelatedRoleID = table.Column<int>(type: "INTEGER", nullable: false),
                    BotConfigID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionRoleConfig", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReactionRoleConfig_Configs_BotConfigID",
                        column: x => x.BotConfigID,
                        principalTable: "Configs",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ReactionRoleConfig_Emojis_RelatedEmojiID",
                        column: x => x.RelatedEmojiID,
                        principalTable: "Emojis",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReactionRoleConfig_Guilds_RelatedGuildId",
                        column: x => x.RelatedGuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReactionRoleConfig_Message_RelatedMessageID",
                        column: x => x.RelatedMessageID,
                        principalTable: "Message",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReactionRoleConfig_Roles_RelatedRoleID",
                        column: x => x.RelatedRoleID,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReactionRoleConfig_BotConfigID",
                table: "ReactionRoleConfig",
                column: "BotConfigID");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionRoleConfig_RelatedEmojiID",
                table: "ReactionRoleConfig",
                column: "RelatedEmojiID");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionRoleConfig_RelatedGuildId",
                table: "ReactionRoleConfig",
                column: "RelatedGuildId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionRoleConfig_RelatedMessageID",
                table: "ReactionRoleConfig",
                column: "RelatedMessageID");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionRoleConfig_RelatedRoleID",
                table: "ReactionRoleConfig",
                column: "RelatedRoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_AntiSpamConfig_Roles_MutedRoleID",
                table: "AntiSpamConfig",
                column: "MutedRoleID",
                principalTable: "Roles",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AntiSpamConfig_Roles_MutedRoleID",
                table: "AntiSpamConfig");

            migrationBuilder.DropTable(
                name: "ReactionRoleConfig");

            migrationBuilder.DropTable(
                name: "Emojis");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Role");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AntiSpamConfig_Role_MutedRoleID",
                table: "AntiSpamConfig",
                column: "MutedRoleID",
                principalTable: "Role",
                principalColumn: "ID");
        }
    }
}
