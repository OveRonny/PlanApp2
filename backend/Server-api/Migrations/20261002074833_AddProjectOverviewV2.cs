using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_api.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectOverviewV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppGoal",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductContext",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetAudience",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppGoal",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProductContext",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TargetAudience",
                table: "Projects");
        }
    }
}
