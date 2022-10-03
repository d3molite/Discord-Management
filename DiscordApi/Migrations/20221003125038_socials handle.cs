using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class socialshandle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserHandle",
                table: "SocialConfigs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserHandle",
                table: "SocialConfigs");
        }
    }
}
