using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Udemy.Migrations
{
    public partial class BannerDesigned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Banners",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Banners");
        }
    }
}
