using Microsoft.EntityFrameworkCore;
using Server_api.Infrastructure.GitHub;
namespace Server_api.Features.GitHub.Commands.ConnectGitHub;

public sealed class ConnectGitHubEndpoint : IEndpoint
{ public void MapEndpoint(IEndpointRouteBuilder endpoints) { endpoints.MapPost("/api/github/connection", async (ConnectGitHubRequest request, AppDbContext db, ICurrentUser currentUser, IGitHubClient gitHub, IDataProtector protector, CancellationToken cancellationToken) => { var profile = await gitHub.ExchangeCodeAsync(request.Code, cancellationToken); var connection = await db.GitHubConnections.SingleOrDefaultAsync(x => x.UserId == currentUser.UserId, cancellationToken); if (connection is null) { connection = new GitHubConnection { UserId = currentUser.UserId, GitHubUserId = profile.Id, GitHubLogin = profile.Login, AccessTokenProtected = protector.Protect(profile.AccessToken) }; db.GitHubConnections.Add(connection); } else { connection.GitHubUserId = profile.Id; connection.GitHubLogin = profile.Login; connection.AccessTokenProtected = protector.Protect(profile.AccessToken); connection.LastUsedAt = DateTime.UtcNow; } await db.SaveChangesAsync(cancellationToken); return Results.Ok(new { connection.Id, connection.GitHubUserId, connection.GitHubLogin }); }).RequireAuthorization().WithTags("GitHub"); } }
public sealed record ConnectGitHubRequest(string Code);
