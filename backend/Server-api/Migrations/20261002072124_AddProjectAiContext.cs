using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_api.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectAiContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(name: "AiContext", table: "Projects", type: "nvarchar(max)", nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "AiContext", table: "Projects");
        }
    }
}
