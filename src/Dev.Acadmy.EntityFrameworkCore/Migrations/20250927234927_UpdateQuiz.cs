using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dev.Acadmy.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppLecturesApp_AppQuizesApp_QuizId",
                table: "AppLecturesApp");

            migrationBuilder.DropIndex(
                name: "IX_AppLecturesApp_QuizId",
                table: "AppLecturesApp");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "AppLecturesApp");

            migrationBuilder.AddColumn<Guid>(
                name: "LectureId",
                table: "AppQuizStudentApp",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LectureId",
                table: "AppQuizesApp",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuizTryCount",
                table: "AppQuizesApp",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AppQuizesApp_LectureId",
                table: "AppQuizesApp",
                column: "LectureId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppQuizesApp_AppLecturesApp_LectureId",
                table: "AppQuizesApp",
                column: "LectureId",
                principalTable: "AppLecturesApp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppQuizesApp_AppLecturesApp_LectureId",
                table: "AppQuizesApp");

            migrationBuilder.DropIndex(
                name: "IX_AppQuizesApp_LectureId",
                table: "AppQuizesApp");

            migrationBuilder.DropColumn(
                name: "LectureId",
                table: "AppQuizStudentApp");

            migrationBuilder.DropColumn(
                name: "LectureId",
                table: "AppQuizesApp");

            migrationBuilder.DropColumn(
                name: "QuizTryCount",
                table: "AppQuizesApp");

            migrationBuilder.AddColumn<Guid>(
                name: "QuizId",
                table: "AppLecturesApp",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AppLecturesApp_QuizId",
                table: "AppLecturesApp",
                column: "QuizId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppLecturesApp_AppQuizesApp_QuizId",
                table: "AppLecturesApp",
                column: "QuizId",
                principalTable: "AppQuizesApp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
