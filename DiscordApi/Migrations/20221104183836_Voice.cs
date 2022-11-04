using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class Voice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VoiceChannelConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OnlyAllowedIn = table.Column<ulong>(type: "INTEGER", nullable: true),
                    CategoryId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    VoiceGuildId = table.Column<int>(type: "INTEGER", nullable: false),
                    BotConfigID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoiceChannelConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoiceChannelConfigs_Configs_BotConfigID",
                        column: x => x.BotConfigID,
                        principalTable: "Configs",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_VoiceChannelConfigs_Guilds_VoiceGuildId",
                        column: x => x.VoiceGuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VoiceChannelConfigs_BotConfigID",
                table: "VoiceChannelConfigs",
                column: "BotConfigID");

            migrationBuilder.CreateIndex(
                name: "IX_VoiceChannelConfigs_VoiceGuildId",
                table: "VoiceChannelConfigs",
                column: "VoiceGuildId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoiceChannelConfigs");
        }
    }
}
