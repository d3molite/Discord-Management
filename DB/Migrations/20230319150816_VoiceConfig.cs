using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class VoiceConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoiceConfigId",
                table: "GuildConfigs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VoiceConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RestrictedChannelId = table.Column<int>(type: "INTEGER", nullable: true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoiceConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoiceConfigs_GuildChannel_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "GuildChannel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VoiceConfigs_GuildChannel_RestrictedChannelId",
                        column: x => x.RestrictedChannelId,
                        principalTable: "GuildChannel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildConfigs_VoiceConfigId",
                table: "GuildConfigs",
                column: "VoiceConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_VoiceConfigs_CategoryId",
                table: "VoiceConfigs",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_VoiceConfigs_RestrictedChannelId",
                table: "VoiceConfigs",
                column: "RestrictedChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildConfigs_VoiceConfigs_VoiceConfigId",
                table: "GuildConfigs",
                column: "VoiceConfigId",
                principalTable: "VoiceConfigs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildConfigs_VoiceConfigs_VoiceConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropTable(
                name: "VoiceConfigs");

            migrationBuilder.DropIndex(
                name: "IX_GuildConfigs_VoiceConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropColumn(
                name: "VoiceConfigId",
                table: "GuildConfigs");
        }
    }
}
