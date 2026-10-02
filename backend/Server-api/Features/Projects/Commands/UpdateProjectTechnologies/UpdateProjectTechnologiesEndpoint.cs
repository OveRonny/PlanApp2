using Microsoft.EntityFrameworkCore;
namespace Server_api.Features.Projects.Commands.UpdateProjectTechnologies;
public sealed class UpdateProjectTechnologiesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/projects/{projectId:guid}/technologies", async (Guid projectId, UpdateProjectTechnologiesRequest request, AppDbContext db, ICurrentUser currentUser, CancellationToken cancellationToken) =>
        {
            var project = await db.Projects.Include(x => x.Workspace).SingleOrDefaultAsync(x => x.Id == projectId, cancellationToken);
            if (project is null) return Results.NotFound();
            if (project.Workspace.OwnerId != currentUser.UserId) return Results.Forbid();
            var technologyIds = request.TechnologyIds.Distinct().ToList();
            var validIds = await db.Technologies.Where(x => technologyIds.Contains(x.Id) && x.IsActive).Select(x => x.Id).ToListAsync(cancellationToken);
            if (validIds.Count != technologyIds.Count) return Results.BadRequest("En eller flere teknologier er ugyldige.");
            var existing = await db.ProjectTechnologies.Where(x => x.ProjectId == projectId).ToListAsync(cancellationToken);
            db.ProjectTechnologies.RemoveRange(existing);
            db.ProjectTechnologies.AddRange(validIds.Select(id => new ProjectTechnology { ProjectId = projectId, TechnologyId = id }));
            await db.SaveChangesAsync(cancellationToken);
            return Results.Ok(new { TechnologyIds = validIds });
        }).RequireAuthorization().WithTags("Projects");
    }
}
public sealed record UpdateProjectTechnologiesRequest(IReadOnlyCollection<Guid> TechnologyIds);
