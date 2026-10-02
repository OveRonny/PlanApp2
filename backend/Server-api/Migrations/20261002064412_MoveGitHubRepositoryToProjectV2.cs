using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_api.Migrations
{
    /// <inheritdoc />
    public partial class MoveGitHubRepositoryToProjectV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkspaceRepositories");

            migrationBuilder.CreateTable(
                name: "ProjectRepositories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GitHubRepositoryId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DefaultBranch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConnectedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRepositories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectRepositories_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRepositories_ProjectId",
                table: "ProjectRepositories",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRepositories_ProjectId_GitHubRepositoryId",
                table: "ProjectRepositories",
                columns: new[] { "ProjectId", "GitHubRepositoryId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectRepositories");

            migrationBuilder.CreateTable(
                name: "WorkspaceRepositories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConnectedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DefaultBranch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    GitHubRepositoryId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceRepositories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkspaceRepositories_Workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceRepositories_WorkspaceId_GitHubRepositoryId",
                table: "WorkspaceRepositories",
                columns: new[] { "WorkspaceId", "GitHubRepositoryId" },
                unique: true);
        }
    }
}
