using Microsoft.EntityFrameworkCore.Migrations;

namespace Todolist.Migrations
{
    public partial class DeleteTasklistUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Tasklists_TasklistId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Tasklists");

            migrationBuilder.AlterColumn<int>(
                name: "TasklistId",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Tasklists_TasklistId",
                table: "Tasks",
                column: "TasklistId",
                principalTable: "Tasklists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Tasklists_TasklistId",
                table: "Tasks");

            migrationBuilder.AlterColumn<int>(
                name: "TasklistId",
                table: "Tasks",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Tasklists",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Tasklists_TasklistId",
                table: "Tasks",
                column: "TasklistId",
                principalTable: "Tasklists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
