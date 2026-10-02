using Microsoft.EntityFrameworkCore;
namespace Server_api.Features.Projects.Commands.DeleteTask;
public sealed class DeleteTaskEndpoint : IEndpoint
{
 public void MapEndpoint(IEndpointRouteBuilder endpoints) { endpoints.MapDelete("/api/projects/{projectId:guid}/tasks/{taskId:guid}", async (Guid projectId, Guid taskId, AppDbContext db, ICurrentUser user, CancellationToken ct) => { var task = await db.WorkItems.Include(x => x.Project).ThenInclude(x => x.Workspace).SingleOrDefaultAsync(x => x.Id == taskId && x.ProjectId == projectId, ct); if (task is null) return Results.NotFound(); if (task.Project.Workspace.OwnerId != user.UserId) return Results.Forbid(); await db.ActivityLogs.Where(x => x.WorkItemId == taskId).ExecuteUpdateAsync(setters => setters.SetProperty(x => x.WorkItemId, (Guid?)null), ct); db.WorkItems.Remove(task); db.ActivityLogs.Add(new ActivityLog { ProjectId = projectId, UserId = user.UserId, Action = "TaskDeleted", Description = $"Oppgaven '{task.Title}' ble slettet." }); await db.SaveChangesAsync(ct); return Results.NoContent(); }).RequireAuthorization().WithTags("Projects"); }
}
