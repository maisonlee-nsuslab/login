using Microsoft.EntityFrameworkCore.Migrations;

namespace Register.Migrations
{
    public partial class pw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserPassword",
                table: "Info",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConfirmPassword",
                table: "Info",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserPassword",
                table: "Info",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConfirmPassword",
                table: "Info",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true);
        }
    }
}
