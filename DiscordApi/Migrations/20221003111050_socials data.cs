using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class socialsdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SocialMediaConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LastPosted = table.Column<ulong>(type: "INTEGER", nullable: false),
                    ChannelId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    BotConfigID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMediaConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialMediaConfig_Configs_BotConfigID",
                        column: x => x.BotConfigID,
                        principalTable: "Configs",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocialMediaConfig_BotConfigID",
                table: "SocialMediaConfig",
                column: "BotConfigID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocialMediaConfig");
        }
    }
}
