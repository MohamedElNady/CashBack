using Microsoft.EntityFrameworkCore.Migrations;

namespace Cash_Back.Migrations
{
    public partial class AddNewTabelForCardItemsnewrow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "discount",
                table: "CardItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discount",
                table: "CardItems");
        }
    }
}
