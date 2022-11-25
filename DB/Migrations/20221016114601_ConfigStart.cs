using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    public partial class ConfigStart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildConfig_Bots_BotId",
                table: "GuildConfig");

            migrationBuilder.DropForeignKey(
                name: "FK_GuildConfig_Guild_LinkedGuildId",
                table: "GuildConfig");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GuildConfig",
                table: "GuildConfig");

            migrationBuilder.RenameTable(
                name: "GuildConfig",
                newName: "GuildConfigs");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Guild",
                newName: "Snowflake");

            migrationBuilder.RenameIndex(
                name: "IX_GuildConfig_LinkedGuildId",
                table: "GuildConfigs",
                newName: "IX_GuildConfigs_LinkedGuildId");

            migrationBuilder.RenameIndex(
                name: "IX_GuildConfig_BotId",
                table: "GuildConfigs",
                newName: "IX_GuildConfigs_BotId");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Bots",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Bots",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AntiSpamConfigId",
                table: "GuildConfigs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FeedbackConfigId",
                table: "GuildConfigs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GuildConfigs",
                table: "GuildConfigs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AntiSpamConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntiSpamConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuildChannel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChannelType = table.Column<int>(type: "INTEGER", nullable: false),
                    Snowflake = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildChannel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsReactionsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    TargetChannelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedbackConfig_GuildChannel_TargetChannelId",
                        column: x => x.TargetChannelId,
                        principalTable: "GuildChannel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildConfigs_AntiSpamConfigId",
                table: "GuildConfigs",
                column: "AntiSpamConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildConfigs_FeedbackConfigId",
                table: "GuildConfigs",
                column: "FeedbackConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackConfig_TargetChannelId",
                table: "FeedbackConfig",
                column: "TargetChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildConfigs_AntiSpamConfig_AntiSpamConfigId",
                table: "GuildConfigs",
                column: "AntiSpamConfigId",
                principalTable: "AntiSpamConfig",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildConfigs_Bots_BotId",
                table: "GuildConfigs",
                column: "BotId",
                principalTable: "Bots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildConfigs_FeedbackConfig_FeedbackConfigId",
                table: "GuildConfigs",
                column: "FeedbackConfigId",
                principalTable: "FeedbackConfig",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildConfigs_Guild_LinkedGuildId",
                table: "GuildConfigs",
                column: "LinkedGuildId",
                principalTable: "Guild",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildConfigs_AntiSpamConfig_AntiSpamConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_GuildConfigs_Bots_BotId",
                table: "GuildConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_GuildConfigs_FeedbackConfig_FeedbackConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_GuildConfigs_Guild_LinkedGuildId",
                table: "GuildConfigs");

            migrationBuilder.DropTable(
                name: "AntiSpamConfig");

            migrationBuilder.DropTable(
                name: "FeedbackConfig");

            migrationBuilder.DropTable(
                name: "GuildChannel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GuildConfigs",
                table: "GuildConfigs");

            migrationBuilder.DropIndex(
                name: "IX_GuildConfigs_AntiSpamConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropIndex(
                name: "IX_GuildConfigs_FeedbackConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropColumn(
                name: "AntiSpamConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropColumn(
                name: "FeedbackConfigId",
                table: "GuildConfigs");

            migrationBuilder.RenameTable(
                name: "GuildConfigs",
                newName: "GuildConfig");

            migrationBuilder.RenameColumn(
                name: "Snowflake",
                table: "Guild",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_GuildConfigs_LinkedGuildId",
                table: "GuildConfig",
                newName: "IX_GuildConfig_LinkedGuildId");

            migrationBuilder.RenameIndex(
                name: "IX_GuildConfigs_BotId",
                table: "GuildConfig",
                newName: "IX_GuildConfig_BotId");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Bots",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Bots",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GuildConfig",
                table: "GuildConfig",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildConfig_Bots_BotId",
                table: "GuildConfig",
                column: "BotId",
                principalTable: "Bots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildConfig_Guild_LinkedGuildId",
                table: "GuildConfig",
                column: "LinkedGuildId",
                principalTable: "Guild",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
