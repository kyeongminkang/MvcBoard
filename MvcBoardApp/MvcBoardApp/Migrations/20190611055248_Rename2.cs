using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcBoardApp.Migrations
{
    public partial class Rename2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Board_BoardID",
                table: "Comment");

            migrationBuilder.AlterColumn<int>(
                name: "BoardID",
                table: "Comment",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Board_BoardID",
                table: "Comment",
                column: "BoardID",
                principalTable: "Board",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Board_BoardID",
                table: "Comment");

            migrationBuilder.AlterColumn<int>(
                name: "BoardID",
                table: "Comment",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Board_BoardID",
                table: "Comment",
                column: "BoardID",
                principalTable: "Board",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
