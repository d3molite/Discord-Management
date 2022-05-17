using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordManager.Migrations
{
    public partial class movedlogging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableLogging",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "LogMessageDeleted",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "LogUserBanned",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "LogUserJoined",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "LogUserKicked",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "LogUserLeft",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "LoggingChannelID",
                table: "Configs");

            migrationBuilder.AddColumn<int>(
                name: "RelatedLoggerID",
                table: "Configs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Bots",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Presence",
                table: "Bots",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Bots",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "LoggingConfigModel",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnableLogging = table.Column<bool>(type: "INTEGER", nullable: false),
                    LogMessageDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    LogUserJoined = table.Column<bool>(type: "INTEGER", nullable: false),
                    LogUserLeft = table.Column<bool>(type: "INTEGER", nullable: false),
                    LogUserKicked = table.Column<bool>(type: "INTEGER", nullable: false),
                    LogUserBanned = table.Column<bool>(type: "INTEGER", nullable: false),
                    LoggingChannelID = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoggingConfigModel", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configs_RelatedLoggerID",
                table: "Configs",
                column: "RelatedLoggerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Configs_LoggingConfigModel_RelatedLoggerID",
                table: "Configs",
                column: "RelatedLoggerID",
                principalTable: "LoggingConfigModel",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Configs_LoggingConfigModel_RelatedLoggerID",
                table: "Configs");

            migrationBuilder.DropTable(
                name: "LoggingConfigModel");

            migrationBuilder.DropIndex(
                name: "IX_Configs_RelatedLoggerID",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "RelatedLoggerID",
                table: "Configs");

            migrationBuilder.AddColumn<bool>(
                name: "EnableLogging",
                table: "Configs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LogMessageDeleted",
                table: "Configs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LogUserBanned",
                table: "Configs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LogUserJoined",
                table: "Configs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LogUserKicked",
                table: "Configs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LogUserLeft",
                table: "Configs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<ulong>(
                name: "LoggingChannelID",
                table: "Configs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Bots",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Presence",
                table: "Bots",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Bots",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
