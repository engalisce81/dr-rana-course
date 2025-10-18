using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dev.Acadmy.Migrations
{
    /// <inheritdoc />
    public partial class ExamBank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppQuestionBanksApp_CourseId",
                table: "AppQuestionBanksApp");

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "AppExamsProgres",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppExamQuestionBanksProgres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionBankId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppExamQuestionBanksProgres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppExamQuestionBanksProgres_AppExamsProgres_ExamId",
                        column: x => x.ExamId,
                        principalTable: "AppExamsProgres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppExamQuestionBanksProgres_AppQuestionBanksApp_QuestionBan~",
                        column: x => x.QuestionBankId,
                        principalTable: "AppQuestionBanksApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppQuestionBanksApp_CourseId",
                table: "AppQuestionBanksApp",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExamQuestionBanksProgres_ExamId",
                table: "AppExamQuestionBanksProgres",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExamQuestionBanksProgres_QuestionBankId",
                table: "AppExamQuestionBanksProgres",
                column: "QuestionBankId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppExamQuestionBanksProgres");

            migrationBuilder.DropIndex(
                name: "IX_AppQuestionBanksApp_CourseId",
                table: "AppQuestionBanksApp");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "AppExamsProgres");

            migrationBuilder.CreateIndex(
                name: "IX_AppQuestionBanksApp_CourseId",
                table: "AppQuestionBanksApp",
                column: "CourseId",
                unique: true);
        }
    }
}
