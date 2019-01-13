using Microsoft.EntityFrameworkCore.Migrations;

namespace Todolist.Migrations
{
    public partial class TasklistUserAddMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "Tasklists",
                maxLength: 100,
                nullable: false
                );

            migrationBuilder.AddForeignKey(
                name: "FK_Tasklists_AspNetUsers_User",
                table: "Tasklists", 
                column: "User",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User",
                table: "Tasklists");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasklists_AspNetUsers_User",
                table: "Tasklists");
        }
    }
}
