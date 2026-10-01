using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_api.Migrations
{
    /// <inheritdoc />
    public partial class AddGitHubWorkspaceMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GitHubConnections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GitHubUserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GitHubLogin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AccessTokenProtected = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUsedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GitHubConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GitHubConnections_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GitHubUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GitHubId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Login = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GitHubUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkspaceRepositories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GitHubRepositoryId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DefaultBranch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConnectedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "WorkspaceMembers",
                columns: table => new
                {
                    WorkspaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GitHubUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceMembers", x => new { x.WorkspaceId, x.GitHubUserId });
                    table.ForeignKey(
                        name: "FK_WorkspaceMembers_GitHubUsers_GitHubUserId",
                        column: x => x.GitHubUserId,
                        principalTable: "GitHubUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkspaceMembers_Workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GitHubConnections_UserId_GitHubUserId",
                table: "GitHubConnections",
                columns: new[] { "UserId", "GitHubUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GitHubUsers_GitHubId",
                table: "GitHubUsers",
                column: "GitHubId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceMembers_GitHubUserId",
                table: "WorkspaceMembers",
                column: "GitHubUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceRepositories_WorkspaceId_GitHubRepositoryId",
                table: "WorkspaceRepositories",
                columns: new[] { "WorkspaceId", "GitHubRepositoryId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GitHubConnections");

            migrationBuilder.DropTable(
                name: "WorkspaceMembers");

            migrationBuilder.DropTable(
                name: "WorkspaceRepositories");

            migrationBuilder.DropTable(
                name: "GitHubUsers");
        }
    }
}
