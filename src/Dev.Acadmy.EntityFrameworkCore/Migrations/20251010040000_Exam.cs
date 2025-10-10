using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dev.Acadmy.Migrations
{
    /// <inheritdoc />
    public partial class Exam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExamId",
                table: "AppQuestionesApp",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppExamsProgres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TimeExam = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppExamsProgres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppExamsProgres_AppCoursesProgres_CourseId",
                        column: x => x.CourseId,
                        principalTable: "AppCoursesProgres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppQuestionesApp_ExamId",
                table: "AppQuestionesApp",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_AppExamsProgres_CourseId",
                table: "AppExamsProgres",
                column: "CourseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppQuestionesApp_AppExamsProgres_ExamId",
                table: "AppQuestionesApp",
                column: "ExamId",
                principalTable: "AppExamsProgres",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppQuestionesApp_AppExamsProgres_ExamId",
                table: "AppQuestionesApp");

            migrationBuilder.DropTable(
                name: "AppExamsProgres");

            migrationBuilder.DropIndex(
                name: "IX_AppQuestionesApp_ExamId",
                table: "AppQuestionesApp");

            migrationBuilder.DropColumn(
                name: "ExamId",
                table: "AppQuestionesApp");
        }
    }
}
