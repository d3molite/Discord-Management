using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class ignoredprefixesonspam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IgnorePrefixes",
                table: "AntiSpamConfig",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IgnorePrefixes",
                table: "AntiSpamConfig");
        }
    }
}
