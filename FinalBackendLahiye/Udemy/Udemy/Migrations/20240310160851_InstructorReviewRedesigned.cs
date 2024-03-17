using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Udemy.Migrations
{
    public partial class InstructorReviewRedesigned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_instructorsReviews",
                table: "instructorsReviews");

            migrationBuilder.RenameTable(
                name: "instructorsReviews",
                newName: "InstructorsReviews");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstructorsReviews",
                table: "InstructorsReviews",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InstructorsReviews",
                table: "InstructorsReviews");

            migrationBuilder.RenameTable(
                name: "InstructorsReviews",
                newName: "instructorsReviews");

            migrationBuilder.AddPrimaryKey(
                name: "PK_instructorsReviews",
                table: "instructorsReviews",
                column: "Id");
        }
    }
}
