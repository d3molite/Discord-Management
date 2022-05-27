using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class nullabilityremoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Guilds_MessageGuildID",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Guilds_RoleGuildID",
                table: "Roles");

            migrationBuilder.AlterColumn<int>(
                name: "RoleGuildID",
                table: "Roles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MessageGuildID",
                table: "Messages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Guilds_MessageGuildID",
                table: "Messages",
                column: "MessageGuildID",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Guilds_RoleGuildID",
                table: "Roles",
                column: "RoleGuildID",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Guilds_MessageGuildID",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Guilds_RoleGuildID",
                table: "Roles");

            migrationBuilder.AlterColumn<int>(
                name: "RoleGuildID",
                table: "Roles",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "MessageGuildID",
                table: "Messages",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Guilds_MessageGuildID",
                table: "Messages",
                column: "MessageGuildID",
                principalTable: "Guilds",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Guilds_RoleGuildID",
                table: "Roles",
                column: "RoleGuildID",
                principalTable: "Guilds",
                principalColumn: "Id");
        }
    }
}
