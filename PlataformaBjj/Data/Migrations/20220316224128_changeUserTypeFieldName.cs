using Microsoft.EntityFrameworkCore.Migrations;

namespace PlataformaBjj.Data.Migrations
{
    public partial class changeUserTypeFieldName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "UserTypes");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserTypes",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserTypes");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "UserTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
