using Microsoft.EntityFrameworkCore;
namespace Server_api.Features.Projects.Commands.UpdateProjectAiContext;
public sealed class UpdateProjectAiContextEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/projects/{projectId:guid}/ai-context", async (Guid projectId, UpdateProjectAiContextRequest request, AppDbContext db, ICurrentUser currentUser, CancellationToken cancellationToken) =>
        { var project = await db.Projects.Include(x => x.Workspace).SingleOrDefaultAsync(x => x.Id == projectId, cancellationToken); if (project is null) return Results.NotFound(); if (project.Workspace.OwnerId != currentUser.UserId) return Results.Forbid(); project.AiContext = request.Context?.Trim(); await db.SaveChangesAsync(cancellationToken); return Results.NoContent(); }).RequireAuthorization().WithTags("AI");
    }
}
public sealed record UpdateProjectAiContextRequest(string? Context);
