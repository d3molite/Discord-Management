using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class socialstwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialMediaConfig_Configs_BotConfigID",
                table: "SocialMediaConfig");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocialMediaConfig",
                table: "SocialMediaConfig");

            migrationBuilder.RenameTable(
                name: "SocialMediaConfig",
                newName: "SocialConfigs");

            migrationBuilder.RenameIndex(
                name: "IX_SocialMediaConfig_BotConfigID",
                table: "SocialConfigs",
                newName: "IX_SocialConfigs_BotConfigID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocialConfigs",
                table: "SocialConfigs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialConfigs_Configs_BotConfigID",
                table: "SocialConfigs",
                column: "BotConfigID",
                principalTable: "Configs",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialConfigs_Configs_BotConfigID",
                table: "SocialConfigs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocialConfigs",
                table: "SocialConfigs");

            migrationBuilder.RenameTable(
                name: "SocialConfigs",
                newName: "SocialMediaConfig");

            migrationBuilder.RenameIndex(
                name: "IX_SocialConfigs_BotConfigID",
                table: "SocialMediaConfig",
                newName: "IX_SocialMediaConfig_BotConfigID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocialMediaConfig",
                table: "SocialMediaConfig",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialMediaConfig_Configs_BotConfigID",
                table: "SocialMediaConfig",
                column: "BotConfigID",
                principalTable: "Configs",
                principalColumn: "ID");
        }
    }
}
