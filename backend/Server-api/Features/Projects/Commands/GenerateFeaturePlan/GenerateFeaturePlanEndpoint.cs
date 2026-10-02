using Microsoft.EntityFrameworkCore;
using Server_api.Infrastructure.OpenAI;
namespace Server_api.Features.Projects.Commands.GenerateFeaturePlan;
public sealed class GenerateFeaturePlanEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/projects/{projectId:guid}/ai/feature-plan", async (Guid projectId, GenerateFeaturePlanRequest request, AppDbContext db, ICurrentUser currentUser, IOpenAiPlanner planner, CancellationToken cancellationToken) =>
        {
            var project = await db.Projects.Include(x => x.Workspace).Include(x => x.Technologies).ThenInclude(x => x.Technology).SingleOrDefaultAsync(x => x.Id == projectId, cancellationToken);
            if (project is null) return Results.NotFound(); if (project.Workspace.OwnerId != currentUser.UserId) return Results.Forbid();
            var technologies = project.Technologies.Select(x => x.Technology.Name).ToList();
            var plan = await planner.GenerateFeaturePlanAsync(project.Name, project.Description ?? string.Empty, technologies, request.Request, cancellationToken);
            var suggestion = new AiPlanSuggestion { ProjectId = projectId, CreatedById = currentUser.UserId, Prompt = request.Request, ProposedPlan = plan, Provider = "OpenAI", Model = "gpt-4o-mini" };
            db.AiPlanSuggestions.Add(suggestion); await db.SaveChangesAsync(cancellationToken);
            return Results.Ok(new { suggestion.Id, suggestion.ProposedPlan });
        }).RequireAuthorization().WithTags("AI");
    }
}
public sealed record GenerateFeaturePlanRequest(string Request);
