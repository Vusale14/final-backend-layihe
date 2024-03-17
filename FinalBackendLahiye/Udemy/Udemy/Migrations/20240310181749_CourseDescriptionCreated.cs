using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Udemy.Migrations
{
    public partial class CourseDescriptionCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseDescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: false)
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
                    ContentText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseDescriptionId = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_CourseDescriptions_CourseId",
                table: "CourseDescriptions",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_DescriptionContents_CourseDescriptionId",
                table: "DescriptionContents",
                column: "CourseDescriptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DescriptionContents");

            migrationBuilder.DropTable(
                name: "CourseDescriptions");
        }
    }
}
