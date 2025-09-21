using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dev.Acadmy.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Rating",
                table: "AppCoursesApp",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "AppCoursesApp",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppSubjectsApp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSubjectsApp", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppCoursesApp_SubjectId",
                table: "AppCoursesApp",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCoursesApp_AppSubjectsApp_SubjectId",
                table: "AppCoursesApp",
                column: "SubjectId",
                principalTable: "AppSubjectsApp",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCoursesApp_AppSubjectsApp_SubjectId",
                table: "AppCoursesApp");

            migrationBuilder.DropTable(
                name: "AppSubjectsApp");

            migrationBuilder.DropIndex(
                name: "IX_AppCoursesApp_SubjectId",
                table: "AppCoursesApp");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "AppCoursesApp");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "AppCoursesApp");
        }
    }
}
