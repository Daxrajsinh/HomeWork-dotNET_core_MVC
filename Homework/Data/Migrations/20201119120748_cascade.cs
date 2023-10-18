using Microsoft.EntityFrameworkCore.Migrations;

namespace Blackboard.Data.Migrations
{
    public partial class cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlackBoards_ClassroomUsers_ClassroomId_AppUserId",
                table: "BlackBoards");

            migrationBuilder.AddForeignKey(
                name: "FK_BlackBoards_ClassroomUsers_ClassroomId_AppUserId",
                table: "BlackBoards",
                columns: new[] { "ClassroomId", "AppUserId" },
                principalTable: "ClassroomUsers",
                principalColumns: new[] { "ClassroomId", "AppUserId" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlackBoards_ClassroomUsers_ClassroomId_AppUserId",
                table: "BlackBoards");

            migrationBuilder.AddForeignKey(
                name: "FK_BlackBoards_ClassroomUsers_ClassroomId_AppUserId",
                table: "BlackBoards",
                columns: new[] { "ClassroomId", "AppUserId" },
                principalTable: "ClassroomUsers",
                principalColumns: new[] { "ClassroomId", "AppUserId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
