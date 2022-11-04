using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class VoiceChannelConfig : Migration
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
                    RelatedGuildId = table.Column<int>(type: "INTEGER", nullable: false),
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
                        name: "FK_VoiceChannelConfigs_Guilds_RelatedGuildId",
                        column: x => x.RelatedGuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VoiceChannelConfigs_BotConfigID",
                table: "VoiceChannelConfigs",
                column: "BotConfigID");

            migrationBuilder.CreateIndex(
                name: "IX_VoiceChannelConfigs_RelatedGuildId",
                table: "VoiceChannelConfigs",
                column: "RelatedGuildId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoiceChannelConfigs");
        }
    }
}
