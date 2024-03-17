using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Udemy.Migrations
{
    public partial class CategoryAndSubcategoryredesigned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExploreText",
                table: "SubCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FAQText",
                table: "SubCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExploreText",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FAQText",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExploreText",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "FAQText",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "ExploreText",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "FAQText",
                table: "Categories");
        }
    }
}
