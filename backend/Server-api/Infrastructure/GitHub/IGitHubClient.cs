namespace Server_api.Infrastructure.GitHub;
public interface IGitHubClient
{
    Task<GitHubProfile> ExchangeCodeAsync(string code, CancellationToken cancellationToken);
    Task<IReadOnlyList<GitHubRepositoryDto>> GetRepositoriesAsync(string accessToken, CancellationToken cancellationToken);
    Task<IReadOnlyList<GitHubCollaboratorDto>> GetCollaboratorsAsync(string accessToken, string fullName, CancellationToken cancellationToken);
}
public sealed record GitHubProfile(string Id, string Login, string? Name, string? AvatarUrl, string AccessToken);
public sealed record GitHubRepositoryDto(long Id, string FullName, string? DefaultBranch);
public sealed record GitHubCollaboratorDto(long Id, string Login, string? Name, string? AvatarUrl);
