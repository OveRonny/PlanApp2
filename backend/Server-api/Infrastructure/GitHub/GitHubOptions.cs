namespace Server_api.Infrastructure.GitHub;

public sealed class GitHubOptions { public const string SectionName = "GitHub"; public string ClientId { get; set; } = string.Empty; public string ClientSecret { get; set; } = string.Empty; public string RedirectUri { get; set; } = string.Empty; }
