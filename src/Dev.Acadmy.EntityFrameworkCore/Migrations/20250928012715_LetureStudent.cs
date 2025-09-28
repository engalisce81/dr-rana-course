using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dev.Acadmy.Migrations
{
    /// <inheritdoc />
    public partial class LetureStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuizTryCount",
                table: "AppLecturesApp",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppLectureStudentsApp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LectureId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttemptsUsed = table.Column<int>(type: "integer", nullable: false),
                    MaxAttempts = table.Column<int>(type: "integer", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLectureStudentsApp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppLectureStudentsApp_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppLectureStudentsApp_AppLecturesApp_LectureId",
                        column: x => x.LectureId,
                        principalTable: "AppLecturesApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppLectureStudentsApp_LectureId",
                table: "AppLectureStudentsApp",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_AppLectureStudentsApp_UserId",
                table: "AppLectureStudentsApp",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppLectureStudentsApp");

            migrationBuilder.DropColumn(
                name: "QuizTryCount",
                table: "AppLecturesApp");
        }
    }
}
