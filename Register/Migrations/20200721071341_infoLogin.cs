using Microsoft.EntityFrameworkCore.Migrations;

namespace Register.Migrations
{
    public partial class infoLogin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "checkPassword",
                table: "Info");

            migrationBuilder.AddColumn<bool>(
                name: "login",
                table: "Info",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "login",
                table: "Info");

            migrationBuilder.AddColumn<string>(
                name: "checkPassword",
                table: "Info",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true);
        }
    }
}
