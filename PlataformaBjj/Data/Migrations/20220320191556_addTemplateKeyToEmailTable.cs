using Microsoft.EntityFrameworkCore.Migrations;

namespace PlataformaBjj.Data.Migrations
{
    public partial class addTemplateKeyToEmailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TemplateKey",
                table: "Emails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplateKey",
                table: "Emails");
        }
    }
}
