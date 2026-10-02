namespace Server_api.Models;

public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Guid WorkspaceId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Workspace Workspace { get; set; } = null!;
    public ICollection<ProjectMember> Members { get; set; } = [];
    public ICollection<Sprint> Sprints { get; set; } = [];
    public ICollection<Feature> Features { get; set; } = [];
    public ICollection<WorkItem> WorkItems { get; set; } = [];
    public ICollection<AiPlanSuggestion> AiPlanSuggestions { get; set; } = [];
    public ICollection<ActivityLog> ActivityLogs { get; set; } = [];
    public ICollection<ProjectSummary> Summaries { get; set; } = [];
    public ProjectRepository? GitHubRepository { get; set; }
}
