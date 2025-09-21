using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dev.Acadmy.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourseInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CollegeId",
                table: "AppSubjectsApp",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppSubjectsApp_CollegeId",
                table: "AppSubjectsApp",
                column: "CollegeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppSubjectsApp_AppCollegesApp_CollegeId",
                table: "AppSubjectsApp",
                column: "CollegeId",
                principalTable: "AppCollegesApp",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSubjectsApp_AppCollegesApp_CollegeId",
                table: "AppSubjectsApp");

            migrationBuilder.DropIndex(
                name: "IX_AppSubjectsApp_CollegeId",
                table: "AppSubjectsApp");

            migrationBuilder.DropColumn(
                name: "CollegeId",
                table: "AppSubjectsApp");
        }
    }
}
