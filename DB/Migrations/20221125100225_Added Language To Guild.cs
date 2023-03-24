using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    public partial class AddedLanguageToGuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackConfig_GuildChannel_TargetChannelId",
                table: "FeedbackConfig");

            migrationBuilder.DropForeignKey(
                name: "FK_GuildConfigs_FeedbackConfig_FeedbackConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedbackConfig",
                table: "FeedbackConfig");

            migrationBuilder.RenameTable(
                name: "FeedbackConfig",
                newName: "FeedbackConfigs");

            migrationBuilder.RenameIndex(
                name: "IX_FeedbackConfig_TargetChannelId",
                table: "FeedbackConfigs",
                newName: "IX_FeedbackConfigs_TargetChannelId");

            migrationBuilder.AddColumn<string>(
                name: "DefaultLanguage",
                table: "Guild",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedbackConfigs",
                table: "FeedbackConfigs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackConfigs_GuildChannel_TargetChannelId",
                table: "FeedbackConfigs",
                column: "TargetChannelId",
                principalTable: "GuildChannel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GuildConfigs_FeedbackConfigs_FeedbackConfigId",
                table: "GuildConfigs",
                column: "FeedbackConfigId",
                principalTable: "FeedbackConfigs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackConfigs_GuildChannel_TargetChannelId",
                table: "FeedbackConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_GuildConfigs_FeedbackConfigs_FeedbackConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedbackConfigs",
                table: "FeedbackConfigs");

            migrationBuilder.DropColumn(
                name: "DefaultLanguage",
                table: "Guild");

            migrationBuilder.RenameTable(
                name: "FeedbackConfigs",
                newName: "FeedbackConfig");

            migrationBuilder.RenameIndex(
                name: "IX_FeedbackConfigs_TargetChannelId",
                table: "FeedbackConfig",
                newName: "IX_FeedbackConfig_TargetChannelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedbackConfig",
                table: "FeedbackConfig",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackConfig_GuildChannel_TargetChannelId",
                table: "FeedbackConfig",
                column: "TargetChannelId",
                principalTable: "GuildChannel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GuildConfigs_FeedbackConfig_FeedbackConfigId",
                table: "GuildConfigs",
                column: "FeedbackConfigId",
                principalTable: "FeedbackConfig",
                principalColumn: "Id");
        }
    }
}
