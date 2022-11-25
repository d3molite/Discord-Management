using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class AddedFaq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FaqConfigId",
                table: "GuildConfigs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<ulong>(
                name: "Snowflake",
                table: "Bots",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.CreateTable(
                name: "FaqConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FaqItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Triggers = table.Column<string>(type: "TEXT", nullable: false),
                    Response = table.Column<string>(type: "TEXT", nullable: false),
                    FaqConfigId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaqItem_FaqConfig_FaqConfigId",
                        column: x => x.FaqConfigId,
                        principalTable: "FaqConfig",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildConfigs_FaqConfigId",
                table: "GuildConfigs",
                column: "FaqConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_FaqItem_FaqConfigId",
                table: "FaqItem",
                column: "FaqConfigId");

            migrationBuilder.AddForeignKey(
                name: "FK_GuildConfigs_FaqConfig_FaqConfigId",
                table: "GuildConfigs",
                column: "FaqConfigId",
                principalTable: "FaqConfig",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildConfigs_FaqConfig_FaqConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropTable(
                name: "FaqItem");

            migrationBuilder.DropTable(
                name: "FaqConfig");

            migrationBuilder.DropIndex(
                name: "IX_GuildConfigs_FaqConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropColumn(
                name: "FaqConfigId",
                table: "GuildConfigs");

            migrationBuilder.DropColumn(
                name: "Snowflake",
                table: "Bots");
        }
    }
}
