using Microsoft.EntityFrameworkCore.Migrations;

namespace PlataformaBjj.Data.Migrations
{
    public partial class addFieldToEmailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Emails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Emails");
        }
    }
}
