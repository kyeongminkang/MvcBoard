using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcBoardApp.Migrations
{
    public partial class BoardCommentName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "C_UserName",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "content",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "WriteTime",
                table: "Board",
                newName: "WriteDate");

            migrationBuilder.RenameColumn(
                name: "Hits",
                table: "Board",
                newName: "CommentCount");

            migrationBuilder.AddColumn<string>(
                name: "CommentContent",
                table: "Comment",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CommentUserName",
                table: "Comment",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Board",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Board",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Board",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentContent",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "CommentUserName",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "WriteDate",
                table: "Board",
                newName: "WriteTime");

            migrationBuilder.RenameColumn(
                name: "CommentCount",
                table: "Board",
                newName: "Hits");

            migrationBuilder.AddColumn<string>(
                name: "C_UserName",
                table: "Comment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "content",
                table: "Comment",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Board",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Board",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Board",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
