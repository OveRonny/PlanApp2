using Microsoft.EntityFrameworkCore;
namespace Server_api.Features.Projects.Queries.GetProjectTechnologies;
public sealed class GetProjectTechnologiesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/projects/{projectId:guid}/technologies", async (Guid projectId, AppDbContext db, ICurrentUser currentUser, CancellationToken cancellationToken) =>
        { var project = await db.Projects.AsNoTracking().Include(x => x.Workspace).SingleOrDefaultAsync(x => x.Id == projectId, cancellationToken); if (project is null) return Results.NotFound(); if (project.Workspace.OwnerId != currentUser.UserId) return Results.Forbid(); return Results.Ok(await db.ProjectTechnologies.AsNoTracking().Where(x => x.ProjectId == projectId).Select(x => x.TechnologyId).ToListAsync(cancellationToken)); }).RequireAuthorization().WithTags("Projects");
    }
}
