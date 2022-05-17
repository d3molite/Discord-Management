using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class ImageManipConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AntiSpamConfig_Role_MutedRoleID",
                table: "AntiSpamConfig");

            migrationBuilder.AddColumn<bool>(
                name: "ImageManipulation",
                table: "Configs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "MutedRoleID",
                table: "AntiSpamConfig",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_AntiSpamConfig_Role_MutedRoleID",
                table: "AntiSpamConfig",
                column: "MutedRoleID",
                principalTable: "Role",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AntiSpamConfig_Role_MutedRoleID",
                table: "AntiSpamConfig");

            migrationBuilder.DropColumn(
                name: "ImageManipulation",
                table: "Configs");

            migrationBuilder.AlterColumn<int>(
                name: "MutedRoleID",
                table: "AntiSpamConfig",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AntiSpamConfig_Role_MutedRoleID",
                table: "AntiSpamConfig",
                column: "MutedRoleID",
                principalTable: "Role",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
