using Microsoft.EntityFrameworkCore.Migrations;

namespace PhoneStore.Data.Migrations
{
    public partial class picturefields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNew",
                table: "Pictures",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SeoFilename",
                table: "Pictures",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNew",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "SeoFilename",
                table: "Pictures");
        }
    }
}
