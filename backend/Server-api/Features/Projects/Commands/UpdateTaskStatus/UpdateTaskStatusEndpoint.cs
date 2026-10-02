using Microsoft.EntityFrameworkCore;
namespace Server_api.Features.Projects.Commands.UpdateTaskStatus;
public sealed class UpdateTaskStatusEndpoint : IEndpoint
{
 public void MapEndpoint(IEndpointRouteBuilder endpoints) { endpoints.MapPut("/api/projects/{projectId:guid}/tasks/{taskId:guid}/status", async (Guid projectId, Guid taskId, UpdateTaskStatusRequest request, AppDbContext db, ICurrentUser user, CancellationToken ct) => { var task = await db.WorkItems.Include(x => x.Project).ThenInclude(x => x.Workspace).SingleOrDefaultAsync(x => x.Id == taskId && x.ProjectId == projectId, ct); if (task is null) return Results.NotFound(); if (task.Project.Workspace.OwnerId != user.UserId) return Results.Forbid(); task.Status = request.Status; db.ActivityLogs.Add(new ActivityLog { ProjectId = projectId, WorkItemId = taskId, UserId = user.UserId, Action = "StatusChanged", Description = $"Oppgavestatus endret til {request.Status}.", Source = AiSource.Manual }); await db.SaveChangesAsync(ct); return Results.Ok(new { task.Id, task.Status }); }).RequireAuthorization().WithTags("Projects"); }
}
public sealed record UpdateTaskStatusRequest(WorkItemStatus Status);
