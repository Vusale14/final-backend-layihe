using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Udemy.Migrations
{
    public partial class BannerDesignedagain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description2",
                table: "Banners",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description3",
                table: "Banners",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description2",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Description3",
                table: "Banners");
        }
    }
}
