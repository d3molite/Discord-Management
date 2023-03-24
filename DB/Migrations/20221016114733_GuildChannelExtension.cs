using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    public partial class GuildChannelExtension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LinkedGuildId",
                table: "GuildChannel",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GuildChannel_LinkedGuildId",
                table: "GuildChannel",
                column: "LinkedGuildId");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildChannel_Guild_LinkedGuildId",
                table: "GuildChannel",
                column: "LinkedGuildId",
                principalTable: "Guild",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildChannel_Guild_LinkedGuildId",
                table: "GuildChannel");

            migrationBuilder.DropIndex(
                name: "IX_GuildChannel_LinkedGuildId",
                table: "GuildChannel");

            migrationBuilder.DropColumn(
                name: "LinkedGuildId",
                table: "GuildChannel");
        }
    }
}
