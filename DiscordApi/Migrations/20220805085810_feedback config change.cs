using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class feedbackconfigchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackConfig_Configs_BotConfigID",
                table: "FeedbackConfig");

            migrationBuilder.DropIndex(
                name: "IX_FeedbackConfig_BotConfigID",
                table: "FeedbackConfig");

            migrationBuilder.DropColumn(
                name: "BotConfigID",
                table: "FeedbackConfig");

            migrationBuilder.AddColumn<int>(
                name: "FeedbackConfigID",
                table: "Configs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Configs_FeedbackConfigID",
                table: "Configs",
                column: "FeedbackConfigID");

            migrationBuilder.AddForeignKey(
                name: "FK_Configs_FeedbackConfig_FeedbackConfigID",
                table: "Configs",
                column: "FeedbackConfigID",
                principalTable: "FeedbackConfig",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Configs_FeedbackConfig_FeedbackConfigID",
                table: "Configs");

            migrationBuilder.DropIndex(
                name: "IX_Configs_FeedbackConfigID",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "FeedbackConfigID",
                table: "Configs");

            migrationBuilder.AddColumn<int>(
                name: "BotConfigID",
                table: "FeedbackConfig",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackConfig_BotConfigID",
                table: "FeedbackConfig",
                column: "BotConfigID");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackConfig_Configs_BotConfigID",
                table: "FeedbackConfig",
                column: "BotConfigID",
                principalTable: "Configs",
                principalColumn: "ID");
        }
    }
}
