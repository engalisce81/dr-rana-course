using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dev.Acadmy.Migrations
{
    /// <inheritdoc />
    public partial class StudentQuizMigNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPdf",
                table: "AppCoursesProgres",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PdfUrl",
                table: "AppCoursesProgres",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AppQuizStudentAnswersApp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuizStudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SelectedAnswerId = table.Column<Guid>(type: "uuid", nullable: true),
                    TextAnswer = table.Column<string>(type: "text", nullable: true),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    ScoreObtained = table.Column<double>(type: "double precision", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppQuizStudentAnswersApp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppQuizStudentAnswersApp_AppQuestionesApp_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "AppQuestionesApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppQuizStudentAnswersApp_AppQuizStudentApp_QuizStudentId",
                        column: x => x.QuizStudentId,
                        principalTable: "AppQuizStudentApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppQuizStudentAnswersApp_QuestionId",
                table: "AppQuizStudentAnswersApp",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppQuizStudentAnswersApp_QuizStudentId",
                table: "AppQuizStudentAnswersApp",
                column: "QuizStudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppQuizStudentAnswersApp");

            migrationBuilder.DropColumn(
                name: "IsPdf",
                table: "AppCoursesProgres");

            migrationBuilder.DropColumn(
                name: "PdfUrl",
                table: "AppCoursesProgres");
        }
    }
}
