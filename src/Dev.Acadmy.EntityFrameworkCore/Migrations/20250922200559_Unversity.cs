using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dev.Acadmy.Migrations
{
    /// <inheritdoc />
    public partial class Unversity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TermId",
                table: "AppSubjectsApp",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UniversityId",
                table: "AppCollegesApp",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppGradeLevelsApp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CollegeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppGradeLevelsApp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppGradeLevelsApp_AppCollegesApp_CollegeId",
                        column: x => x.CollegeId,
                        principalTable: "AppCollegesApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppTermsApp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTermsApp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUniversitesApp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUniversitesApp", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppSubjectsApp_TermId",
                table: "AppSubjectsApp",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCollegesApp_UniversityId",
                table: "AppCollegesApp",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_AppGradeLevelsApp_CollegeId",
                table: "AppGradeLevelsApp",
                column: "CollegeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCollegesApp_AppUniversitesApp_UniversityId",
                table: "AppCollegesApp",
                column: "UniversityId",
                principalTable: "AppUniversitesApp",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppSubjectsApp_AppTermsApp_TermId",
                table: "AppSubjectsApp",
                column: "TermId",
                principalTable: "AppTermsApp",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCollegesApp_AppUniversitesApp_UniversityId",
                table: "AppCollegesApp");

            migrationBuilder.DropForeignKey(
                name: "FK_AppSubjectsApp_AppTermsApp_TermId",
                table: "AppSubjectsApp");

            migrationBuilder.DropTable(
                name: "AppGradeLevelsApp");

            migrationBuilder.DropTable(
                name: "AppTermsApp");

            migrationBuilder.DropTable(
                name: "AppUniversitesApp");

            migrationBuilder.DropIndex(
                name: "IX_AppSubjectsApp_TermId",
                table: "AppSubjectsApp");

            migrationBuilder.DropIndex(
                name: "IX_AppCollegesApp_UniversityId",
                table: "AppCollegesApp");

            migrationBuilder.DropColumn(
                name: "TermId",
                table: "AppSubjectsApp");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "AppCollegesApp");
        }
    }
}
