namespace Server_api.Models;

public class ProjectRepository
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProjectId { get; set; }
    public required string GitHubRepositoryId { get; set; }
    public required string FullName { get; set; }
    public string? DefaultBranch { get; set; }
    public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;

    public Project Project { get; set; } = null!;
}
