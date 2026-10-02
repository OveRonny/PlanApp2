using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server_api.Migrations
{
    /// <inheritdoc />
    public partial class AddFeatureAiContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FeatureId",
                table: "AiPlanSuggestions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AiPlanSuggestions_FeatureId",
                table: "AiPlanSuggestions",
                column: "FeatureId");

            migrationBuilder.AddForeignKey(
                name: "FK_AiPlanSuggestions_Features_FeatureId",
                table: "AiPlanSuggestions",
                column: "FeatureId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AiPlanSuggestions_Features_FeatureId",
                table: "AiPlanSuggestions");

            migrationBuilder.DropIndex(
                name: "IX_AiPlanSuggestions_FeatureId",
                table: "AiPlanSuggestions");

            migrationBuilder.DropColumn(
                name: "FeatureId",
                table: "AiPlanSuggestions");
        }
    }
}
