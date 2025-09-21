using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dev.Acadmy.Migrations
{
    /// <inheritdoc />
    public partial class CourseInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppChaptersApp_AppCoursesApp_CourseId",
                table: "AppChaptersApp");

            migrationBuilder.DropForeignKey(
                name: "FK_AppCoursesApp_AbpUsers_UserId",
                table: "AppCoursesApp");

            migrationBuilder.DropForeignKey(
                name: "FK_AppCoursesApp_AppCollegesApp_CollegeId",
                table: "AppCoursesApp");

            migrationBuilder.DropForeignKey(
                name: "FK_AppCoursesApp_AppCollegesApp_CollegeId1",
                table: "AppCoursesApp");

            migrationBuilder.DropForeignKey(
                name: "FK_AppCoursesApp_AppSubjectsApp_SubjectId",
                table: "AppCoursesApp");

            migrationBuilder.DropForeignKey(
                name: "FK_AppCourseStudentesApp_AppCoursesApp_CourseId",
                table: "AppCourseStudentesApp");

            migrationBuilder.DropForeignKey(
                name: "FK_AppQuestionBanksApp_AppCoursesApp_CourseId",
                table: "AppQuestionBanksApp");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppCoursesApp",
                table: "AppCoursesApp");

            migrationBuilder.RenameTable(
                name: "AppCoursesApp",
                newName: "AppCoursesProgres");

            migrationBuilder.RenameIndex(
                name: "IX_AppCoursesApp_UserId",
                table: "AppCoursesProgres",
                newName: "IX_AppCoursesProgres_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AppCoursesApp_SubjectId",
                table: "AppCoursesProgres",
                newName: "IX_AppCoursesProgres_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_AppCoursesApp_CollegeId1",
                table: "AppCoursesProgres",
                newName: "IX_AppCoursesProgres_CollegeId1");

            migrationBuilder.RenameIndex(
                name: "IX_AppCoursesApp_CollegeId",
                table: "AppCoursesProgres",
                newName: "IX_AppCoursesProgres_CollegeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppCoursesProgres",
                table: "AppCoursesProgres",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppCourseInfosProgres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
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
                    table.PrimaryKey("PK_AppCourseInfosProgres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCourseInfosProgres_AppCoursesProgres_CourseId",
                        column: x => x.CourseId,
                        principalTable: "AppCoursesProgres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppCourseInfosProgres_CourseId",
                table: "AppCourseInfosProgres",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppChaptersApp_AppCoursesProgres_CourseId",
                table: "AppChaptersApp",
                column: "CourseId",
                principalTable: "AppCoursesProgres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppCoursesProgres_AbpUsers_UserId",
                table: "AppCoursesProgres",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppCoursesProgres_AppCollegesApp_CollegeId",
                table: "AppCoursesProgres",
                column: "CollegeId",
                principalTable: "AppCollegesApp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppCoursesProgres_AppCollegesApp_CollegeId1",
                table: "AppCoursesProgres",
                column: "CollegeId1",
                principalTable: "AppCollegesApp",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCoursesProgres_AppSubjectsApp_SubjectId",
                table: "AppCoursesProgres",
                column: "SubjectId",
                principalTable: "AppSubjectsApp",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCourseStudentesApp_AppCoursesProgres_CourseId",
                table: "AppCourseStudentesApp",
                column: "CourseId",
                principalTable: "AppCoursesProgres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppQuestionBanksApp_AppCoursesProgres_CourseId",
                table: "AppQuestionBanksApp",
                column: "CourseId",
                principalTable: "AppCoursesProgres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppChaptersApp_AppCoursesProgres_CourseId",
                table: "AppChaptersApp");

            migrationBuilder.DropForeignKey(
                name: "FK_AppCoursesProgres_AbpUsers_UserId",
                table: "AppCoursesProgres");

            migrationBuilder.DropForeignKey(
                name: "FK_AppCoursesProgres_AppCollegesApp_CollegeId",
                table: "AppCoursesProgres");

            migrationBuilder.DropForeignKey(
                name: "FK_AppCoursesProgres_AppCollegesApp_CollegeId1",
                table: "AppCoursesProgres");

            migrationBuilder.DropForeignKey(
                name: "FK_AppCoursesProgres_AppSubjectsApp_SubjectId",
                table: "AppCoursesProgres");

            migrationBuilder.DropForeignKey(
                name: "FK_AppCourseStudentesApp_AppCoursesProgres_CourseId",
                table: "AppCourseStudentesApp");

            migrationBuilder.DropForeignKey(
                name: "FK_AppQuestionBanksApp_AppCoursesProgres_CourseId",
                table: "AppQuestionBanksApp");

            migrationBuilder.DropTable(
                name: "AppCourseInfosProgres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppCoursesProgres",
                table: "AppCoursesProgres");

            migrationBuilder.RenameTable(
                name: "AppCoursesProgres",
                newName: "AppCoursesApp");

            migrationBuilder.RenameIndex(
                name: "IX_AppCoursesProgres_UserId",
                table: "AppCoursesApp",
                newName: "IX_AppCoursesApp_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AppCoursesProgres_SubjectId",
                table: "AppCoursesApp",
                newName: "IX_AppCoursesApp_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_AppCoursesProgres_CollegeId1",
                table: "AppCoursesApp",
                newName: "IX_AppCoursesApp_CollegeId1");

            migrationBuilder.RenameIndex(
                name: "IX_AppCoursesProgres_CollegeId",
                table: "AppCoursesApp",
                newName: "IX_AppCoursesApp_CollegeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppCoursesApp",
                table: "AppCoursesApp",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppChaptersApp_AppCoursesApp_CourseId",
                table: "AppChaptersApp",
                column: "CourseId",
                principalTable: "AppCoursesApp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppCoursesApp_AbpUsers_UserId",
                table: "AppCoursesApp",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppCoursesApp_AppCollegesApp_CollegeId",
                table: "AppCoursesApp",
                column: "CollegeId",
                principalTable: "AppCollegesApp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppCoursesApp_AppCollegesApp_CollegeId1",
                table: "AppCoursesApp",
                column: "CollegeId1",
                principalTable: "AppCollegesApp",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCoursesApp_AppSubjectsApp_SubjectId",
                table: "AppCoursesApp",
                column: "SubjectId",
                principalTable: "AppSubjectsApp",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCourseStudentesApp_AppCoursesApp_CourseId",
                table: "AppCourseStudentesApp",
                column: "CourseId",
                principalTable: "AppCoursesApp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppQuestionBanksApp_AppCoursesApp_CourseId",
                table: "AppQuestionBanksApp",
                column: "CourseId",
                principalTable: "AppCoursesApp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
