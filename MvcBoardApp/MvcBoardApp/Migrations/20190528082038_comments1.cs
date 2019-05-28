using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcBoardApp.Migrations
{
    public partial class comments1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoardId",
                table: "Comment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_BoardId",
                table: "Comment",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Board_BoardId",
                table: "Comment",
                column: "BoardId",
                principalTable: "Board",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Board_BoardId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_BoardId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "Comment");
        }
    }
}
