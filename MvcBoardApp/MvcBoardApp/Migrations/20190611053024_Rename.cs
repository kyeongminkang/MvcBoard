using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcBoardApp.Migrations
{
    public partial class Rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Board_BoardId",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "BoardId",
                table: "Comment",
                newName: "BoardID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Comment",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_BoardId",
                table: "Comment",
                newName: "IX_Comment_BoardID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Board",
                newName: "ID");

            migrationBuilder.AlterColumn<string>(
                name: "CommentUserName",
                table: "Comment",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommentContent",
                table: "Comment",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BoardID",
                table: "Comment",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "WriteDate",
                table: "Board",
                nullable: true,
                oldClrType: typeof(DateTime));

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

            migrationBuilder.AlterColumn<int>(
                name: "CommentCount",
                table: "Board",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Board_BoardID",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "BoardID",
                table: "Comment",
                newName: "BoardId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Comment",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_BoardID",
                table: "Comment",
                newName: "IX_Comment_BoardId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Board",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "CommentUserName",
                table: "Comment",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CommentContent",
                table: "Comment",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "BoardId",
                table: "Comment",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "WriteDate",
                table: "Board",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

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

            migrationBuilder.AlterColumn<int>(
                name: "CommentCount",
                table: "Board",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Board_BoardId",
                table: "Comment",
                column: "BoardId",
                principalTable: "Board",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
