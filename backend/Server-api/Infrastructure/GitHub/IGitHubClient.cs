namespace Server_api.Infrastructure.GitHub;

public interface IGitHubClient { Task<GitHubProfile> ExchangeCodeAsync(string code, CancellationToken cancellationToken); }
public sealed record GitHubProfile(string Id, string Login, string? Name, string? AvatarUrl, string AccessToken);
