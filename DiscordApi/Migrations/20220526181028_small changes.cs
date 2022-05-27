using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class smallchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReactionRoleConfig_Message_RelatedMessageID",
                table: "ReactionRoleConfig");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.RenameTable(
                name: "Message",
                newName: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "RoleGuildID",
                table: "Roles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MessageGuildID",
                table: "Messages",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleGuildID",
                table: "Roles",
                column: "RoleGuildID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageGuildID",
                table: "Messages",
                column: "MessageGuildID");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Guilds_MessageGuildID",
                table: "Messages",
                column: "MessageGuildID",
                principalTable: "Guilds",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionRoleConfig_Messages_RelatedMessageID",
                table: "ReactionRoleConfig",
                column: "RelatedMessageID",
                principalTable: "Messages",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Guilds_RoleGuildID",
                table: "Roles",
                column: "RoleGuildID",
                principalTable: "Guilds",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Guilds_MessageGuildID",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_ReactionRoleConfig_Messages_RelatedMessageID",
                table: "ReactionRoleConfig");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Guilds_RoleGuildID",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_RoleGuildID",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_MessageGuildID",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "RoleGuildID",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "MessageGuildID",
                table: "Messages");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "Message");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ReactionRoleConfig_Message_RelatedMessageID",
                table: "ReactionRoleConfig",
                column: "RelatedMessageID",
                principalTable: "Message",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
