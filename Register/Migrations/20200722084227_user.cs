using Microsoft.EntityFrameworkCore.Migrations;

namespace Register.Migrations
{
    public partial class user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Info",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(maxLength: 10, nullable: true),
                    UserName = table.Column<string>(maxLength: 10, nullable: true),
                    UserPassword = table.Column<string>(maxLength: 8, nullable: true),
                    isLogin = table.Column<bool>(nullable: false),
                    ConfirmPassword = table.Column<string>(maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Info", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Info");
        }
    }
}
