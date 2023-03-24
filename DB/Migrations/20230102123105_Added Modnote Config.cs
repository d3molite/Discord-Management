using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class AddedModnoteConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildChannel_Guild_LinkedGuildId",
                table: "GuildChannel");

            migrationBuilder.DropForeignKey(
                name: "FK_GuildConfigs_Guild_LinkedGuildId",
                table: "GuildConfigs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Guild",
                table: "Guild");

            migrationBuilder.RenameTable(
                name: "Guild",
                newName: "Guilds");

            migrationBuilder.AddColumn<int>(
                name: "ModnoteConfigId",
                table: "GuildConfigs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Guilds",
                table: "Guilds",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ModnoteConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    MinimumHierarchy = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModnoteConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Snowflake = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modnotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GuildId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuthorId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateLogged = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modnotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modnotes_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Modnotes_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Modnotes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildConfigs_ModnoteConfigId",
                table: "GuildConfigs",
                column: "ModnoteConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Modnotes_AuthorId",
                table: "Modnotes",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Modnotes_GuildId",
                table: "Modnotes",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Modnotes_UserId",
                table: "Modnotes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildChannel_Guilds_LinkedGuildId",
                table: "GuildChannel",
                column: "LinkedGuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GuildConfigs_Guilds_LinkedGuildId",
                table: "GuildConfigs",
                column: "LinkedGuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GuildConfigs_ModnoteConfigs_ModnoteConfigId",
                table: "GuildConfigs",
                column: "ModnoteConfigId",
                principalTable: "ModnoteConfigs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildChannel_Guilds_LinkedGuildId",
                table: "GuildChannel");

            migrationBuilder.DropForeignKey(
                name: "FK_GuildConfigs_Guilds_LinkedGuildId",
                table: "GuildConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_GuildConfigs_ModnoteConfigs_ModnoteConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropTable(
                name: "ModnoteConfigs");

            migrationBuilder.DropTable(
                name: "Modnotes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_GuildConfigs_ModnoteConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Guilds",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "ModnoteConfigId",
                table: "GuildConfigs");

            migrationBuilder.RenameTable(
                name: "Guilds",
                newName: "Guild");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Guild",
                table: "Guild",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildChannel_Guild_LinkedGuildId",
                table: "GuildChannel",
                column: "LinkedGuildId",
                principalTable: "Guild",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GuildConfigs_Guild_LinkedGuildId",
                table: "GuildConfigs",
                column: "LinkedGuildId",
                principalTable: "Guild",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
