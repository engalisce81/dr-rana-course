using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dev.Acadmy.Migrations
{
    /// <inheritdoc />
    public partial class ExamManyCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppExamsProgres_CourseId",
                table: "AppExamsProgres");

            migrationBuilder.CreateIndex(
                name: "IX_AppExamsProgres_CourseId",
                table: "AppExamsProgres",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppExamsProgres_CourseId",
                table: "AppExamsProgres");

            migrationBuilder.CreateIndex(
                name: "IX_AppExamsProgres_CourseId",
                table: "AppExamsProgres",
                column: "CourseId",
                unique: true);
        }
    }
}
