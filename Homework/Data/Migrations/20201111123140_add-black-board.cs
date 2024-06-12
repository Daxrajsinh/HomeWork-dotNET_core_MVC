using Microsoft.EntityFrameworkCore.Migrations;

namespace Homework.Data.Migrations
{
    public partial class addHomework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Homeworks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassroomId = table.Column<int>(nullable: false),
                    AppUserId = table.Column<string>(nullable: true),
                    content = table.Column<string>(nullable: true),
                    FilesPaths = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homeworks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Homeworks_ClassroomUsers_ClassroomId_AppUserId",
                        columns: x => new { x.ClassroomId, x.AppUserId },
                        principalTable: "ClassroomUsers",
                        principalColumns: new[] { "ClassroomId", "AppUserId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_ClassroomId_AppUserId",
                table: "Homeworks",
                columns: new[] { "ClassroomId", "AppUserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Homeworks");
        }
    }
}
