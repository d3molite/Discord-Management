using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class modnoteauthor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Modnotes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Logged",
                table: "Modnotes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateIndex(
                name: "IX_Modnotes_AuthorId",
                table: "Modnotes",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modnotes_Users_AuthorId",
                table: "Modnotes",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modnotes_Users_AuthorId",
                table: "Modnotes");

            migrationBuilder.DropIndex(
                name: "IX_Modnotes_AuthorId",
                table: "Modnotes");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Modnotes");

            migrationBuilder.DropColumn(
                name: "Logged",
                table: "Modnotes");
        }
    }
}
