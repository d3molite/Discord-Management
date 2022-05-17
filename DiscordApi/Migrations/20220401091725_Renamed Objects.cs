using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class RenamedObjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Configs_LoggingConfigModel_RelatedLoggerID",
                table: "Configs");

            migrationBuilder.DropForeignKey(
                name: "FK_Guilds_Bots_BotModelID",
                table: "Guilds");

            migrationBuilder.DropTable(
                name: "LoggingConfigModel");

            migrationBuilder.RenameColumn(
                name: "ServerID",
                table: "Guilds",
                newName: "GuildID");

            migrationBuilder.RenameColumn(
                name: "BotModelID",
                table: "Guilds",
                newName: "BotID");

            migrationBuilder.RenameIndex(
                name: "IX_Guilds_BotModelID",
                table: "Guilds",
                newName: "IX_Guilds_BotID");

            migrationBuilder.CreateTable(
                name: "LoggingConfig",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                    table.PrimaryKey("PK_LoggingConfig", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Configs_LoggingConfig_RelatedLoggerID",
                table: "Configs",
                column: "RelatedLoggerID",
                principalTable: "LoggingConfig",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Guilds_Bots_BotID",
                table: "Guilds",
                column: "BotID",
                principalTable: "Bots",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Configs_LoggingConfig_RelatedLoggerID",
                table: "Configs");

            migrationBuilder.DropForeignKey(
                name: "FK_Guilds_Bots_BotID",
                table: "Guilds");

            migrationBuilder.DropTable(
                name: "LoggingConfig");

            migrationBuilder.RenameColumn(
                name: "GuildID",
                table: "Guilds",
                newName: "ServerID");

            migrationBuilder.RenameColumn(
                name: "BotID",
                table: "Guilds",
                newName: "BotModelID");

            migrationBuilder.RenameIndex(
                name: "IX_Guilds_BotID",
                table: "Guilds",
                newName: "IX_Guilds_BotModelID");

            migrationBuilder.CreateTable(
                name: "LoggingConfigModel",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnableLogging = table.Column<bool>(type: "INTEGER", nullable: false),
                    LogMessageDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    LogUserBanned = table.Column<bool>(type: "INTEGER", nullable: false),
                    LogUserJoined = table.Column<bool>(type: "INTEGER", nullable: false),
                    LogUserKicked = table.Column<bool>(type: "INTEGER", nullable: false),
                    LogUserLeft = table.Column<bool>(type: "INTEGER", nullable: false),
                    LoggingChannelID = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoggingConfigModel", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Configs_LoggingConfigModel_RelatedLoggerID",
                table: "Configs",
                column: "RelatedLoggerID",
                principalTable: "LoggingConfigModel",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Guilds_Bots_BotModelID",
                table: "Guilds",
                column: "BotModelID",
                principalTable: "Bots",
                principalColumn: "ID");
        }
    }
}
