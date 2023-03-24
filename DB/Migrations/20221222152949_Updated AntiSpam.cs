using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAntiSpam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoggingConfigId",
                table: "GuildConfigs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IgnorePrefixes",
                table: "AntiSpamConfig",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MutedRoleId",
                table: "AntiSpamConfig",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GuildRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Snowflake = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoggingConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LoggingChannelId = table.Column<int>(type: "INTEGER", nullable: false),
                    LogMessageDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    LogUserJoined = table.Column<bool>(type: "INTEGER", nullable: false),
                    LogUserLeft = table.Column<bool>(type: "INTEGER", nullable: false),
                    LogUserBanned = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoggingConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoggingConfig_GuildChannel_LoggingChannelId",
                        column: x => x.LoggingChannelId,
                        principalTable: "GuildChannel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildConfigs_LoggingConfigId",
                table: "GuildConfigs",
                column: "LoggingConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_AntiSpamConfig_MutedRoleId",
                table: "AntiSpamConfig",
                column: "MutedRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_LoggingConfig_LoggingChannelId",
                table: "LoggingConfig",
                column: "LoggingChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AntiSpamConfig_GuildRole_MutedRoleId",
                table: "AntiSpamConfig",
                column: "MutedRoleId",
                principalTable: "GuildRole",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildConfigs_LoggingConfig_LoggingConfigId",
                table: "GuildConfigs",
                column: "LoggingConfigId",
                principalTable: "LoggingConfig",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AntiSpamConfig_GuildRole_MutedRoleId",
                table: "AntiSpamConfig");

            migrationBuilder.DropForeignKey(
                name: "FK_GuildConfigs_LoggingConfig_LoggingConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropTable(
                name: "GuildRole");

            migrationBuilder.DropTable(
                name: "LoggingConfig");

            migrationBuilder.DropIndex(
                name: "IX_GuildConfigs_LoggingConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropIndex(
                name: "IX_AntiSpamConfig_MutedRoleId",
                table: "AntiSpamConfig");

            migrationBuilder.DropColumn(
                name: "LoggingConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropColumn(
                name: "IgnorePrefixes",
                table: "AntiSpamConfig");

            migrationBuilder.DropColumn(
                name: "MutedRoleId",
                table: "AntiSpamConfig");
        }
    }
}
