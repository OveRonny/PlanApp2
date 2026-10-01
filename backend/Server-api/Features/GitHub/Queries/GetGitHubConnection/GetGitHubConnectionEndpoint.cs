using Microsoft.EntityFrameworkCore;
namespace Server_api.Features.GitHub.Queries.GetGitHubConnection;

public sealed class GetGitHubConnectionEndpoint : IEndpoint
{ public void MapEndpoint(IEndpointRouteBuilder endpoints) { endpoints.MapGet("/api/github/connection", async (AppDbContext db, ICurrentUser currentUser, CancellationToken cancellationToken) => { var connection = await db.GitHubConnections.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == currentUser.UserId, cancellationToken); return connection is null ? Results.NoContent() : Results.Ok(new { connection.Id, connection.GitHubUserId, connection.GitHubLogin, connection.CreatedAt, connection.LastUsedAt }); }).RequireAuthorization().WithTags("GitHub"); } }
