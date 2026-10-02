using Microsoft.EntityFrameworkCore;

namespace Server_api.Features.Projects.Queries.GetProjectOverview;

public sealed class GetProjectOverviewEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/projects/{projectId:guid}/overview", async (Guid projectId, AppDbContext db, ICurrentUser currentUser, CancellationToken cancellationToken) =>
        {
            var project = await db.Projects.AsNoTracking().Include(x => x.Workspace).SingleOrDefaultAsync(x => x.Id == projectId, cancellationToken);
            if (project is null) return Results.NotFound();
            if (project.Workspace.OwnerId != currentUser.UserId) return Results.Forbid();
            return Results.Ok(new { project.AppGoal, project.TargetAudience, project.ProductContext });
        }).RequireAuthorization().WithTags("Projects");
    }
}
