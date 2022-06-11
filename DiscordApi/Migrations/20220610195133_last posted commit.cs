using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class lastpostedcommit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastCommitPosted",
                table: "Configs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastCommitPosted",
                table: "Configs");
        }
    }
}
