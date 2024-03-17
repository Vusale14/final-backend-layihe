using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Udemy.Migrations
{
    public partial class AppUserCoursesRedesigned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsersCourses_AspNetUsers_AppUserId",
                table: "AppUsersCourses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUsersCourses",
                table: "AppUsersCourses");

            migrationBuilder.DropIndex(
                name: "IX_AppUsersCourses_AppUserId",
                table: "AppUsersCourses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AppUsersCourses");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "AppUsersCourses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUsersCourses",
                table: "AppUsersCourses",
                columns: new[] { "AppUserId", "CourseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsersCourses_AspNetUsers_AppUserId",
                table: "AppUsersCourses",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsersCourses_AspNetUsers_AppUserId",
                table: "AppUsersCourses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUsersCourses",
                table: "AppUsersCourses");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "AppUsersCourses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AppUsersCourses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUsersCourses",
                table: "AppUsersCourses",
                columns: new[] { "UserId", "CourseId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsersCourses_AppUserId",
                table: "AppUsersCourses",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsersCourses_AspNetUsers_AppUserId",
                table: "AppUsersCourses",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
