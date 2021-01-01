using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartAccess.Api.Migrations
{
    public partial class OpenRequestModify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "OpenRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "OpenRequests");
        }
    }
}
