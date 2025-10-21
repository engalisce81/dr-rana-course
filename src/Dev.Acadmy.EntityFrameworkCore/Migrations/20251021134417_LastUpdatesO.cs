using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dev.Acadmy.Migrations
{
    /// <inheritdoc />
    public partial class LastUpdatesO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppQuestionesApp_AppExamsProgres_ExamId",
                table: "AppQuestionesApp");

            migrationBuilder.DropIndex(
                name: "IX_AppQuestionesApp_ExamId",
                table: "AppQuestionesApp");

            migrationBuilder.DropColumn(
                name: "ExamId",
                table: "AppQuestionesApp");

            migrationBuilder.AddColumn<bool>(
                name: "IsSucces",
                table: "AppLectureTrysApp",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SuccessQuizRate",
                table: "AppLecturesApp",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppExamQuestionsApp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppExamQuestionsApp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppExamQuestionsApp_AppExamsProgres_ExamId",
                        column: x => x.ExamId,
                        principalTable: "AppExamsProgres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppExamQuestionsApp_AppQuestionesApp_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "AppQuestionesApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppSupportsApp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FaceBook = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSupportsApp", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppExamQuestionsApp_ExamId",
                table: "AppExamQuestionsApp",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExamQuestionsApp_QuestionId",
                table: "AppExamQuestionsApp",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppExamQuestionsApp");

            migrationBuilder.DropTable(
                name: "AppSupportsApp");

            migrationBuilder.DropColumn(
                name: "IsSucces",
                table: "AppLectureTrysApp");

            migrationBuilder.DropColumn(
                name: "SuccessQuizRate",
                table: "AppLecturesApp");

            migrationBuilder.AddColumn<Guid>(
                name: "ExamId",
                table: "AppQuestionesApp",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppQuestionesApp_ExamId",
                table: "AppQuestionesApp",
                column: "ExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppQuestionesApp_AppExamsProgres_ExamId",
                table: "AppQuestionesApp",
                column: "ExamId",
                principalTable: "AppExamsProgres",
                principalColumn: "Id");
        }
    }
}
