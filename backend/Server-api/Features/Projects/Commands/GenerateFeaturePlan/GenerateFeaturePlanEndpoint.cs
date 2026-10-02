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
            var plan = await planner.GenerateFeaturePlanAsync(project.Name, $"{project.Description}\nAI-kontekst: {project.AiContext}", technologies, $"Område: {request.Category}. Teknologi: {request.TechnologyId}. {request.Request}", cancellationToken);
            var suggestion = new AiPlanSuggestion { ProjectId = projectId, CreatedById = currentUser.UserId, Prompt = request.Request, ProposedPlan = plan, Provider = "OpenAI", Model = "gpt-4o-mini" };
            db.AiPlanSuggestions.Add(suggestion); await db.SaveChangesAsync(cancellationToken);
            try
            {
                var jsonText = plan.Trim();
                if (jsonText.StartsWith("```"))
                {
                    var firstLineEnd = jsonText.IndexOf('\n');
                    if (firstLineEnd >= 0) jsonText = jsonText[(firstLineEnd + 1)..];
                    jsonText = jsonText.TrimEnd();
                    if (jsonText.EndsWith("```")) jsonText = jsonText[..^3].TrimEnd();
                }

                var jsonStart = jsonText.IndexOf('{');
                var jsonEnd = jsonText.LastIndexOf('}');
                if (jsonStart >= 0 && jsonEnd > jsonStart) jsonText = jsonText[jsonStart..(jsonEnd + 1)];
                using var json = System.Text.Json.JsonDocument.Parse(jsonText);
                if (json.RootElement.TryGetProperty("tasks", out var taskList) && taskList.ValueKind == System.Text.Json.JsonValueKind.Array)
                {
                    foreach (var task in taskList.EnumerateArray())
                    {
                        db.Set<AiTaskSuggestion>().Add(new AiTaskSuggestion
                        {
                            AiPlanSuggestionId = suggestion.Id,
                            Title = task.TryGetProperty("title", out var title) ? title.GetString() ?? "Oppgave" : "Oppgave",
                            Description = task.TryGetProperty("description", out var description) ? description.GetString() : null,
                            Context = task.TryGetProperty("context", out var context) ? context.GetString() : null,
                            TechnologyId = request.TechnologyId,
                            Category = request.Category
                        });
                    }
                    await db.SaveChangesAsync(cancellationToken);
                }
            }
            catch (System.Text.Json.JsonException) { }
            return Results.Ok(new { suggestion.Id, suggestion.ProposedPlan });
        }).RequireAuthorization().WithTags("AI");
    }
}
public sealed record GenerateFeaturePlanRequest(string Request, TechnologyCategory Category, Guid? TechnologyId);
