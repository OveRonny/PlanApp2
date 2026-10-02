using Microsoft.EntityFrameworkCore;
namespace Server_api.Features.Projects.Queries.GetFeatures;
public sealed class GetFeaturesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/projects/{projectId:guid}/features", async (Guid projectId, AppDbContext db, ICurrentUser currentUser, CancellationToken cancellationToken) =>
        {
            var project = await db.Projects.AsNoTracking().Include(x => x.Workspace).SingleOrDefaultAsync(x => x.Id == projectId, cancellationToken);
            if (project is null) return Results.NotFound(); if (project.Workspace.OwnerId != currentUser.UserId) return Results.Forbid();
            var features = await db.Features.AsNoTracking().Where(x => x.ProjectId == projectId).Include(x => x.WorkItems).OrderBy(x => x.Title).Select(x => new { x.Id, x.Title, x.Description, x.Status, x.AiSource, Tasks = x.WorkItems.OrderBy(w => w.CreatedAt).Select(w => new { w.Id, w.Title, w.Description, w.Status, w.Type, w.Priority, w.AiSource }) }).ToListAsync(cancellationToken);
            return Results.Ok(features);
        }).RequireAuthorization().WithTags("Projects");
    }
}
