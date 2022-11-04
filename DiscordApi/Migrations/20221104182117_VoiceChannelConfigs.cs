using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class VoiceChannelConfigs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoiceChannelConfigs_Configs_BotConfigID",
                table: "VoiceChannelConfigs");

            migrationBuilder.DropIndex(
                name: "IX_VoiceChannelConfigs_BotConfigID",
                table: "VoiceChannelConfigs");

            migrationBuilder.DropColumn(
                name: "BotConfigID",
                table: "VoiceChannelConfigs");

            migrationBuilder.AddColumn<int>(
                name: "VoiceConfigId",
                table: "Configs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Configs_VoiceConfigId",
                table: "Configs",
                column: "VoiceConfigId");

            migrationBuilder.AddForeignKey(
                name: "FK_Configs_VoiceChannelConfigs_VoiceConfigId",
                table: "Configs",
                column: "VoiceConfigId",
                principalTable: "VoiceChannelConfigs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Configs_VoiceChannelConfigs_VoiceConfigId",
                table: "Configs");

            migrationBuilder.DropIndex(
                name: "IX_Configs_VoiceConfigId",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "VoiceConfigId",
                table: "Configs");

            migrationBuilder.AddColumn<int>(
                name: "BotConfigID",
                table: "VoiceChannelConfigs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoiceChannelConfigs_BotConfigID",
                table: "VoiceChannelConfigs",
                column: "BotConfigID");

            migrationBuilder.AddForeignKey(
                name: "FK_VoiceChannelConfigs_Configs_BotConfigID",
                table: "VoiceChannelConfigs",
                column: "BotConfigID",
                principalTable: "Configs",
                principalColumn: "ID");
        }
    }
}
