namespace Server_api.Models;

public class GitHubUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string GitHubId { get; set; }
    public required string Login { get; set; }
    public string? DisplayName { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<WorkspaceMember> WorkspaceMemberships { get; set; } = [];
}
