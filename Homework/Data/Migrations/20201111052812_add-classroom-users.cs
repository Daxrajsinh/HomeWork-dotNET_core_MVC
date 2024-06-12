using Microsoft.EntityFrameworkCore.Migrations;

namespace Homework.Data.Migrations
{
    public partial class addclassroomusers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassroomUsers",
                columns: table => new
                {
                    ClassroomId = table.Column<int>(nullable: false),
                    AppUserId = table.Column<string>(nullable: false),
                    Role = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomUsers", x => new { x.ClassroomId, x.AppUserId });
                    table.ForeignKey(
                        name: "FK_ClassroomUsers_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassroomUsers_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomUsers_AppUserId",
                table: "ClassroomUsers",
                column: "AppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassroomUsers");
        }
    }
}
