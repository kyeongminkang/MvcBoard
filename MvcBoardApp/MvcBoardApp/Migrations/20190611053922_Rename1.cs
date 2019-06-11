using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcBoardApp.Migrations
{
    public partial class Rename1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CommentCount",
                table: "Board",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CommentCount",
                table: "Board",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
