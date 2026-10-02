using Microsoft.EntityFrameworkCore;
namespace Server_api.Features.Projects.Commands.ReviewAiTask;
public sealed class ReviewAiTaskEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/ai/task-suggestions/{id:guid}", async (Guid id, ReviewAiTaskRequest request, AppDbContext db, ICurrentUser currentUser, CancellationToken cancellationToken) =>
        {
            var task = await db.Set<AiTaskSuggestion>().Include(x => x.AiPlanSuggestion).ThenInclude(x => x.Project).ThenInclude(x => x.Workspace).SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (task is null) return Results.NotFound();
            if (task.AiPlanSuggestion.Project.Workspace.OwnerId != currentUser.UserId) return Results.Forbid();
            task.ApprovalStatus = request.Approved ? AiApprovalStatus.Approved : AiApprovalStatus.Rejected;
            if (request.Approved)
            {
                var suggestion = task.AiPlanSuggestion;
                var feature = suggestion.FeatureId.HasValue ? await db.Features.SingleAsync(x => x.Id == suggestion.FeatureId.Value, cancellationToken) : null;
                if (feature is null)
                {
                    var title = "AI-feature: " + suggestion.Prompt.Trim();
                    try { var planText = suggestion.ProposedPlan.Trim(); var start = planText.IndexOf('{'); var end = planText.LastIndexOf('}'); if (start >= 0 && end > start) planText = planText[start..(end + 1)]; using var json = System.Text.Json.JsonDocument.Parse(planText); if (json.RootElement.TryGetProperty("title", out var titleElement)) title = titleElement.GetString() ?? title; } catch (System.Text.Json.JsonException) { }
                    if (title.Length > 200) title = title[..200];
                    feature = new Feature { ProjectId = suggestion.ProjectId, Title = title, Description = "Opprettet fra AI-forslag.", AiSource = AiSource.AiGenerated, AiContext = suggestion.Prompt };
                    db.Features.Add(feature); suggestion.Feature = feature;
                }
                if (!await db.WorkItems.AnyAsync(x => x.FeatureId == feature.Id && x.Title == task.Title, cancellationToken)) db.WorkItems.Add(new WorkItem { ProjectId = suggestion.ProjectId, Feature = feature, Title = task.Title, Description = task.Description, AiSource = AiSource.AiGenerated, AiContext = task.Context });
            }
            await db.SaveChangesAsync(cancellationToken);
            return Results.Ok(new { task.Id, task.ApprovalStatus, featureId = task.AiPlanSuggestion.FeatureId });
        }).RequireAuthorization().WithTags("AI");
    }
}
public sealed record ReviewAiTaskRequest(bool Approved);
