using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dev.Acadmy.Migrations
{
    /// <inheritdoc />
    public partial class GradeLevelsUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GradeLevelId",
                table: "AppSubjectsApp",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppSubjectsApp_GradeLevelId",
                table: "AppSubjectsApp",
                column: "GradeLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppSubjectsApp_AppGradeLevelsApp_GradeLevelId",
                table: "AppSubjectsApp",
                column: "GradeLevelId",
                principalTable: "AppGradeLevelsApp",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSubjectsApp_AppGradeLevelsApp_GradeLevelId",
                table: "AppSubjectsApp");

            migrationBuilder.DropIndex(
                name: "IX_AppSubjectsApp_GradeLevelId",
                table: "AppSubjectsApp");

            migrationBuilder.DropColumn(
                name: "GradeLevelId",
                table: "AppSubjectsApp");
        }
    }
}
