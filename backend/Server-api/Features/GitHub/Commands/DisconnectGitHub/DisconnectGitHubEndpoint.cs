using Microsoft.EntityFrameworkCore;
namespace Server_api.Features.GitHub.Commands.DisconnectGitHub;
public sealed class DisconnectGitHubEndpoint : IEndpoint
{ public void MapEndpoint(IEndpointRouteBuilder endpoints) { endpoints.MapDelete("/api/github/connection", async (AppDbContext db, ICurrentUser currentUser, CancellationToken cancellationToken) => { var connection = await db.GitHubConnections.SingleOrDefaultAsync(x => x.UserId == currentUser.UserId, cancellationToken); if (connection is not null) { db.GitHubConnections.Remove(connection); await db.SaveChangesAsync(cancellationToken); } return Results.NoContent(); }).RequireAuthorization().WithTags("GitHub"); } }
