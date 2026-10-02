using Microsoft.EntityFrameworkCore;
namespace Server_api.Features.Projects.Queries.GetProjectTasks;
public sealed class GetProjectTasksEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/projects/{projectId:guid}/tasks", async (Guid projectId, AppDbContext db, ICurrentUser currentUser, CancellationToken cancellationToken) =>
        {
            var project = await db.Projects.AsNoTracking().Include(x => x.Workspace).SingleOrDefaultAsync(x => x.Id == projectId, cancellationToken);
            if (project is null) return Results.NotFound();
            if (project.Workspace.OwnerId != currentUser.UserId) return Results.Forbid();
            var tasks = await db.WorkItems.AsNoTracking().Where(x => x.ProjectId == projectId).OrderBy(x => x.Status).ThenByDescending(x => x.CreatedAt).Select(x => new { x.Id, x.Title, x.Description, x.Status, x.Priority, x.Type, x.FeatureId, FeatureTitle = x.Feature == null ? null : x.Feature.Title, x.AiSource }).ToListAsync(cancellationToken);
            return Results.Ok(tasks);
        }).RequireAuthorization().WithTags("Projects");
    }
}
