using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class modnotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageManipulation",
                table: "Configs",
                newName: "ModnotesEnabled");

            migrationBuilder.AddColumn<bool>(
                name: "ImageManipulationEnabled",
                table: "Configs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageManipulationEnabled",
                table: "Configs");

            migrationBuilder.RenameColumn(
                name: "ModnotesEnabled",
                table: "Configs",
                newName: "ImageManipulation");
        }
    }
}
