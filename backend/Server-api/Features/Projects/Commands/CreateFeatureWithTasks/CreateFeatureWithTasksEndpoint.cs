using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Server_api.Features.Projects.Commands.CreateFeatureWithTasks;

public sealed class CreateFeatureWithTasksEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/projects/{projectId:guid}/features/with-tasks", async (Guid projectId, CreateFeatureWithTasksRequest request, AppDbContext db, ICurrentUser currentUser, CancellationToken cancellationToken) =>
        {
            var project = await db.Projects.Include(x => x.Workspace).SingleOrDefaultAsync(x => x.Id == projectId, cancellationToken);
            if (project is null) return Results.NotFound();
            if (project.Workspace.OwnerId != currentUser.UserId) return Results.Forbid();
            if (string.IsNullOrWhiteSpace(request.Title) || request.Tasks.Count == 0) return Results.BadRequest("En feature må ha tittel og minst én oppgave.");

            await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
            var feature = new Feature { Title = request.Title.Trim(), Description = request.Description?.Trim(), ProjectId = projectId, AiSource = AiSource.AiGenerated, AiContext = request.Context };
            db.Features.Add(feature);
            foreach (var task in request.Tasks.Where(x => !string.IsNullOrWhiteSpace(x.Title)))
                db.WorkItems.Add(new WorkItem { Title = task.Title.Trim(), Description = task.Description?.Trim(), ProjectId = projectId, Feature = feature, Type = WorkItemType.Task, AiSource = AiSource.AiGenerated, AiContext = JsonSerializer.Serialize(new { feature = feature.Title, project = project.Name, task.Context }) });
            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return Results.Created($"/api/features/{feature.Id}", new { feature.Id, feature.Title, TaskCount = request.Tasks.Count });
        }).RequireAuthorization().WithTags("Projects");
    }
}

public sealed record CreateFeatureWithTasksRequest(string Title, string? Description, IReadOnlyCollection<GeneratedTaskRequest> Tasks, string? Context);
public sealed record GeneratedTaskRequest(string Title, string? Description, string? Context);
