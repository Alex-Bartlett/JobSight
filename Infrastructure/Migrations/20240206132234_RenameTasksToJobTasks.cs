using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameTasksToJobTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskImages_Tasks_JobTaskId",
                table: "TaskImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Jobs_JobId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_UserId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskImages",
                table: "TaskImages");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "JobTasks");

            migrationBuilder.RenameTable(
                name: "TaskImages",
                newName: "JobTaskImages");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_UserId",
                table: "JobTasks",
                newName: "IX_JobTasks_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_JobId",
                table: "JobTasks",
                newName: "IX_JobTasks_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskImages_JobTaskId",
                table: "JobTaskImages",
                newName: "IX_JobTaskImages_JobTaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobTasks",
                table: "JobTasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobTaskImages",
                table: "JobTaskImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobTaskImages_JobTasks_JobTaskId",
                table: "JobTaskImages",
                column: "JobTaskId",
                principalTable: "JobTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobTasks_Jobs_JobId",
                table: "JobTasks",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobTasks_Users_UserId",
                table: "JobTasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobTaskImages_JobTasks_JobTaskId",
                table: "JobTaskImages");

            migrationBuilder.DropForeignKey(
                name: "FK_JobTasks_Jobs_JobId",
                table: "JobTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_JobTasks_Users_UserId",
                table: "JobTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobTasks",
                table: "JobTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobTaskImages",
                table: "JobTaskImages");

            migrationBuilder.RenameTable(
                name: "JobTasks",
                newName: "Tasks");

            migrationBuilder.RenameTable(
                name: "JobTaskImages",
                newName: "TaskImages");

            migrationBuilder.RenameIndex(
                name: "IX_JobTasks_UserId",
                table: "Tasks",
                newName: "IX_Tasks_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_JobTasks_JobId",
                table: "Tasks",
                newName: "IX_Tasks_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_JobTaskImages_JobTaskId",
                table: "TaskImages",
                newName: "IX_TaskImages_JobTaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskImages",
                table: "TaskImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskImages_Tasks_JobTaskId",
                table: "TaskImages",
                column: "JobTaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Jobs_JobId",
                table: "Tasks",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_UserId",
                table: "Tasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
