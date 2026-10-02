using Microsoft.EntityFrameworkCore;
namespace Server_api.Features.Projects.Queries.GetProjectTask;
public sealed class GetProjectTaskEndpoint : IEndpoint
{
 public void MapEndpoint(IEndpointRouteBuilder endpoints) { endpoints.MapGet("/api/projects/{projectId:guid}/tasks/{taskId:guid}", async (Guid projectId, Guid taskId, AppDbContext db, ICurrentUser user, CancellationToken ct) => { var task = await db.WorkItems.AsNoTracking().Where(x => x.ProjectId == projectId && x.Id == taskId).Select(x => new { x.Id, x.Title, x.Description, x.Status, x.Priority, x.Type, x.AiSource, x.FeatureId, FeatureTitle = x.Feature == null ? null : x.Feature.Title, Activities = x.ActivityLogs.OrderByDescending(a => a.CreatedAt).Select(a => new { a.Id, a.Action, a.Description, a.CreatedAt }) }).SingleOrDefaultAsync(ct); if (task is null) return Results.NotFound(); var allowed = await db.Projects.Include(x => x.Workspace).AnyAsync(x => x.Id == projectId && x.Workspace.OwnerId == user.UserId, ct); return allowed ? Results.Ok(task) : Results.Forbid(); }).RequireAuthorization().WithTags("Projects"); }
}
