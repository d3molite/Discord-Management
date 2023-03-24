using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedASC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AntiSpamConfig_GuildRole_MutedRoleId",
                table: "AntiSpamConfig");

            migrationBuilder.DropForeignKey(
                name: "FK_GuildConfigs_AntiSpamConfig_AntiSpamConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AntiSpamConfig",
                table: "AntiSpamConfig");

            migrationBuilder.RenameTable(
                name: "AntiSpamConfig",
                newName: "AntiSpamConfigs");

            migrationBuilder.RenameIndex(
                name: "IX_AntiSpamConfig_MutedRoleId",
                table: "AntiSpamConfigs",
                newName: "IX_AntiSpamConfigs_MutedRoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AntiSpamConfigs",
                table: "AntiSpamConfigs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AntiSpamConfigs_GuildRole_MutedRoleId",
                table: "AntiSpamConfigs",
                column: "MutedRoleId",
                principalTable: "GuildRole",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildConfigs_AntiSpamConfigs_AntiSpamConfigId",
                table: "GuildConfigs",
                column: "AntiSpamConfigId",
                principalTable: "AntiSpamConfigs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AntiSpamConfigs_GuildRole_MutedRoleId",
                table: "AntiSpamConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_GuildConfigs_AntiSpamConfigs_AntiSpamConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AntiSpamConfigs",
                table: "AntiSpamConfigs");

            migrationBuilder.RenameTable(
                name: "AntiSpamConfigs",
                newName: "AntiSpamConfig");

            migrationBuilder.RenameIndex(
                name: "IX_AntiSpamConfigs_MutedRoleId",
                table: "AntiSpamConfig",
                newName: "IX_AntiSpamConfig_MutedRoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AntiSpamConfig",
                table: "AntiSpamConfig",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AntiSpamConfig_GuildRole_MutedRoleId",
                table: "AntiSpamConfig",
                column: "MutedRoleId",
                principalTable: "GuildRole",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildConfigs_AntiSpamConfig_AntiSpamConfigId",
                table: "GuildConfigs",
                column: "AntiSpamConfigId",
                principalTable: "AntiSpamConfig",
                principalColumn: "Id");
        }
    }
}
