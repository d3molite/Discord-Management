using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class AddedAntiSpam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guilds_Bots_BotID",
                table: "Guilds");

            migrationBuilder.DropIndex(
                name: "IX_Guilds_BotID",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "BotID",
                table: "Guilds");

            migrationBuilder.AddColumn<int>(
                name: "AntiSpamID",
                table: "Configs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    RoleID = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AntiSpamConfig",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MessageLimit = table.Column<int>(type: "INTEGER", nullable: false),
                    LinkLimit = table.Column<int>(type: "INTEGER", nullable: false),
                    ResetTime = table.Column<int>(type: "INTEGER", nullable: false),
                    MutedRoleID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntiSpamConfig", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AntiSpamConfig_Role_MutedRoleID",
                        column: x => x.MutedRoleID,
                        principalTable: "Role",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configs_AntiSpamID",
                table: "Configs",
                column: "AntiSpamID");

            migrationBuilder.CreateIndex(
                name: "IX_AntiSpamConfig_MutedRoleID",
                table: "AntiSpamConfig",
                column: "MutedRoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Configs_AntiSpamConfig_AntiSpamID",
                table: "Configs",
                column: "AntiSpamID",
                principalTable: "AntiSpamConfig",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Configs_AntiSpamConfig_AntiSpamID",
                table: "Configs");

            migrationBuilder.DropTable(
                name: "AntiSpamConfig");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Configs_AntiSpamID",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "AntiSpamID",
                table: "Configs");

            migrationBuilder.AddColumn<int>(
                name: "BotID",
                table: "Guilds",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_BotID",
                table: "Guilds",
                column: "BotID");

            migrationBuilder.AddForeignKey(
                name: "FK_Guilds_Bots_BotID",
                table: "Guilds",
                column: "BotID",
                principalTable: "Bots",
                principalColumn: "ID");
        }
    }
}
