using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class AddedImageManipulationtoConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ImageManipulationEnabled",
                table: "GuildConfigs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Snowflake = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReactionRoleConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MessageId = table.Column<int>(type: "INTEGER", nullable: true),
                    GuildConfigId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionRoleConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReactionRoleConfigs_GuildConfigs_GuildConfigId",
                        column: x => x.GuildConfigId,
                        principalTable: "GuildConfigs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReactionRoleConfigs_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Message",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReactionRoleItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmojiId = table.Column<int>(type: "INTEGER", nullable: true),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReactionRoleConfigId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactionRoleItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReactionRoleItem_Emojis_EmojiId",
                        column: x => x.EmojiId,
                        principalTable: "Emojis",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReactionRoleItem_GuildRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "GuildRole",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReactionRoleItem_ReactionRoleConfigs_ReactionRoleConfigId",
                        column: x => x.ReactionRoleConfigId,
                        principalTable: "ReactionRoleConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReactionRoleConfigs_GuildConfigId",
                table: "ReactionRoleConfigs",
                column: "GuildConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionRoleConfigs_MessageId",
                table: "ReactionRoleConfigs",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionRoleItem_EmojiId",
                table: "ReactionRoleItem",
                column: "EmojiId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionRoleItem_ReactionRoleConfigId",
                table: "ReactionRoleItem",
                column: "ReactionRoleConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ReactionRoleItem_RoleId",
                table: "ReactionRoleItem",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReactionRoleItem");

            migrationBuilder.DropTable(
                name: "ReactionRoleConfigs");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropColumn(
                name: "ImageManipulationEnabled",
                table: "GuildConfigs");
        }
    }
}
