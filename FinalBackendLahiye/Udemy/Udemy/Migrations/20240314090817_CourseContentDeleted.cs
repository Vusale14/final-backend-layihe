using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Udemy.Migrations
{
    public partial class CourseContentDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseContents");

            migrationBuilder.DropTable(
                name: "DescriptionContents");

            migrationBuilder.DropTable(
                name: "CourseDescriptions");

            migrationBuilder.AddColumn<string>(
                name: "ContentLevel",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LectureCount",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalHours",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentLevel",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "LectureCount",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TotalHours",
                table: "Courses");

            migrationBuilder.CreateTable(
                name: "CourseContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    ContentText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseContents_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseDescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseDescriptions_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DescriptionContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseDescriptionId = table.Column<int>(type: "int", nullable: false),
                    ContentText = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DescriptionContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DescriptionContents_CourseDescriptions_CourseDescriptionId",
                        column: x => x.CourseDescriptionId,
                        principalTable: "CourseDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseContents_CourseId",
                table: "CourseContents",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseDescriptions_CourseId",
                table: "CourseDescriptions",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_DescriptionContents_CourseDescriptionId",
                table: "DescriptionContents",
                column: "CourseDescriptionId");
        }
    }
}
