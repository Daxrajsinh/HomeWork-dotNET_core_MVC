using Microsoft.EntityFrameworkCore.Migrations;

namespace Homework.Data.Migrations
{
    public partial class cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_ClassroomUsers_ClassroomId_AppUserId",
                table: "Homeworks");

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_ClassroomUsers_ClassroomId_AppUserId",
                table: "Homeworks",
                columns: new[] { "ClassroomId", "AppUserId" },
                principalTable: "ClassroomUsers",
                principalColumns: new[] { "ClassroomId", "AppUserId" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_ClassroomUsers_ClassroomId_AppUserId",
                table: "Homeworks");

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_ClassroomUsers_ClassroomId_AppUserId",
                table: "Homeworks",
                columns: new[] { "ClassroomId", "AppUserId" },
                principalTable: "ClassroomUsers",
                principalColumns: new[] { "ClassroomId", "AppUserId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
