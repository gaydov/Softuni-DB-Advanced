using Microsoft.EntityFrameworkCore.Migrations;

namespace P03_FootballBetting.Migrations
{
    public partial class TeamNameAndInitialsUnicode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Initials",
                table: "Teams",
                type: "NCHAR(3)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "CHAR(3)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Initials",
                table: "Teams",
                type: "CHAR(3)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NCHAR(3)");
        }
    }
}
