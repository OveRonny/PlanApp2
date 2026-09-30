namespace Server_api.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Email { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Workspace> OwnedWorkspaces { get; set; } = [];
    public ICollection<ProjectMember> ProjectMemberships { get; set; } = [];
    public ICollection<WorkItem> AssignedWorkItems { get; set; } = [];
    public ICollection<AiPlanSuggestion> AiPlanSuggestions { get; set; } = [];
    public ICollection<ActivityLog> ActivityLogs { get; set; } = [];
}
