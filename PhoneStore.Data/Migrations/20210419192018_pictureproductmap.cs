using Microsoft.EntityFrameworkCore.Migrations;

namespace PhoneStore.Data.Migrations
{
    public partial class pictureproductmap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Pictures",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Pictures",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
