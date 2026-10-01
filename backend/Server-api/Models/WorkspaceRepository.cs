namespace Server_api.Models;

public class WorkspaceRepository
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid WorkspaceId { get; set; }
    public required string GitHubRepositoryId { get; set; }
    public required string FullName { get; set; }
    public string? DefaultBranch { get; set; }
    public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;
    public Workspace Workspace { get; set; } = null!;
}
