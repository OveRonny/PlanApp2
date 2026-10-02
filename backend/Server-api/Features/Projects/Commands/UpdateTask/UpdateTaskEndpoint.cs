using Microsoft.EntityFrameworkCore;
namespace Server_api.Features.Projects.Commands.UpdateTask;
public sealed class UpdateTaskEndpoint : IEndpoint
{
 public void MapEndpoint(IEndpointRouteBuilder endpoints) { endpoints.MapPut("/api/projects/{projectId:guid}/tasks/{taskId:guid}", async (Guid projectId, Guid taskId, UpdateTaskRequest request, AppDbContext db, ICurrentUser user, CancellationToken ct) => { var task = await db.WorkItems.Include(x => x.Project).ThenInclude(x => x.Workspace).SingleOrDefaultAsync(x => x.Id == taskId && x.ProjectId == projectId, ct); if (task is null) return Results.NotFound(); if (task.Project.Workspace.OwnerId != user.UserId) return Results.Forbid(); task.Title = request.Title.Trim(); task.Description = request.Description?.Trim(); task.Priority = request.Priority; db.ActivityLogs.Add(new ActivityLog { ProjectId = projectId, WorkItemId = taskId, UserId = user.UserId, Action = "TaskUpdated", Description = "Oppgaven ble redigert." }); await db.SaveChangesAsync(ct); return Results.NoContent(); }).RequireAuthorization().WithTags("Projects"); }
}
public sealed record UpdateTaskRequest(string Title, string? Description, WorkItemPriority Priority);
