namespace Server_api.Models;

public class GitHubConnection
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public required string GitHubUserId { get; set; }
    public required string GitHubLogin { get; set; }
    public string? AccessTokenProtected { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastUsedAt { get; set; }
    public User User { get; set; } = null!;
}
