using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server_api.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectTechnologies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Technologies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technologies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTechnologies",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TechnologyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTechnologies", x => new { x.ProjectId, x.TechnologyId });
                    table.ForeignKey(
                        name: "FK_ProjectTechnologies_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTechnologies_Technologies_TechnologyId",
                        column: x => x.TechnologyId,
                        principalTable: "Technologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Technologies",
                columns: new[] { "Id", "Category", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), 0, true, "Vue" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), 0, true, "React" },
                    { new Guid("10000000-0000-0000-0000-000000000003"), 0, true, "TypeScript" },
                    { new Guid("10000000-0000-0000-0000-000000000004"), 1, true, "ASP.NET Core" },
                    { new Guid("10000000-0000-0000-0000-000000000005"), 1, true, "Node.js" },
                    { new Guid("10000000-0000-0000-0000-000000000006"), 2, true, "SQL Server" },
                    { new Guid("10000000-0000-0000-0000-000000000007"), 2, true, "PostgreSQL" },
                    { new Guid("10000000-0000-0000-0000-000000000008"), 3, true, "Docker" },
                    { new Guid("10000000-0000-0000-0000-000000000009"), 3, true, "GitHub Actions" },
                    { new Guid("10000000-0000-0000-0000-000000000010"), 4, true, "xUnit" },
                    { new Guid("10000000-0000-0000-0000-000000000011"), 4, true, "Vitest" },
                    { new Guid("10000000-0000-0000-0000-000000000012"), 5, true, "REST" },
                    { new Guid("10000000-0000-0000-0000-000000000013"), 5, true, "GraphQL" },
                    { new Guid("10000000-0000-0000-0000-000000000014"), 6, true, "Git" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTechnologies_TechnologyId",
                table: "ProjectTechnologies",
                column: "TechnologyId");

            migrationBuilder.CreateIndex(
                name: "IX_Technologies_Name_Category",
                table: "Technologies",
                columns: new[] { "Name", "Category" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTechnologies");

            migrationBuilder.DropTable(
                name: "Technologies");
        }
    }
}
