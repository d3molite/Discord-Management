using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bots",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Token = table.Column<string>(type: "TEXT", nullable: true),
                    Presence = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bots", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ServerID = table.Column<int>(type: "INTEGER", nullable: false),
                    BotModelID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guilds_Bots_BotModelID",
                        column: x => x.BotModelID,
                        principalTable: "Bots",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RelatedGuildId = table.Column<int>(type: "INTEGER", nullable: false),
                    RelatedBotID = table.Column<int>(type: "INTEGER", nullable: false),
                    EnableLogging = table.Column<bool>(type: "INTEGER", nullable: false),
                    LogMessageDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    LogUserJoined = table.Column<bool>(type: "INTEGER", nullable: false),
                    LogUserLeft = table.Column<bool>(type: "INTEGER", nullable: false),
                    LogUserKicked = table.Column<bool>(type: "INTEGER", nullable: false),
                    LogUserBanned = table.Column<bool>(type: "INTEGER", nullable: false),
                    LoggingChannelID = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Configs_Bots_RelatedBotID",
                        column: x => x.RelatedBotID,
                        principalTable: "Bots",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Configs_Guilds_RelatedGuildId",
                        column: x => x.RelatedGuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configs_RelatedBotID",
                table: "Configs",
                column: "RelatedBotID");

            migrationBuilder.CreateIndex(
                name: "IX_Configs_RelatedGuildId",
                table: "Configs",
                column: "RelatedGuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_BotModelID",
                table: "Guilds",
                column: "BotModelID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "Guilds");

            migrationBuilder.DropTable(
                name: "Bots");
        }
    }
}
