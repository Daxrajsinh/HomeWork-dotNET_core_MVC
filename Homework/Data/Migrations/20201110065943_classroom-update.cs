using Microsoft.EntityFrameworkCore.Migrations;

namespace Homework.Data.Migrations
{
    public partial class classroomupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classrooms_AspNetUsers_AppUserId",
                table: "Classrooms");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Classrooms");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Classrooms",
                newName: "AppUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Classrooms_AppUserId",
                table: "Classrooms",
                newName: "IX_Classrooms_AppUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Classrooms_AspNetUsers_AppUserID",
                table: "Classrooms",
                column: "AppUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classrooms_AspNetUsers_AppUserID",
                table: "Classrooms");

            migrationBuilder.RenameColumn(
                name: "AppUserID",
                table: "Classrooms",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Classrooms_AppUserID",
                table: "Classrooms",
                newName: "IX_Classrooms_AppUserId");

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Classrooms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Classrooms_AspNetUsers_AppUserId",
                table: "Classrooms",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
