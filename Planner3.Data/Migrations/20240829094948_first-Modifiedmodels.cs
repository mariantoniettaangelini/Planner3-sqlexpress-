using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner3.Data.Migrations
{
    /// <inheritdoc />
    public partial class firstModifiedmodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Progress_WorkoutSession_WorkoutSessionId",
                table: "Progress");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Progress");

            migrationBuilder.AlterColumn<int>(
                name: "WorkoutSessionId",
                table: "Progress",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Progress_WorkoutSession_WorkoutSessionId",
                table: "Progress",
                column: "WorkoutSessionId",
                principalTable: "WorkoutSession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Progress_WorkoutSession_WorkoutSessionId",
                table: "Progress");

            migrationBuilder.AlterColumn<int>(
                name: "WorkoutSessionId",
                table: "Progress",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SessionId",
                table: "Progress",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Progress_WorkoutSession_WorkoutSessionId",
                table: "Progress",
                column: "WorkoutSessionId",
                principalTable: "WorkoutSession",
                principalColumn: "Id");
        }
    }
}
