using Microsoft.AspNetCore.Identity;

namespace Server_api.Models;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Workspace> OwnedWorkspaces { get; set; } = [];
    public ICollection<ProjectMember> ProjectMemberships { get; set; } = [];
    public ICollection<WorkItem> AssignedWorkItems { get; set; } = [];
    public ICollection<AiPlanSuggestion> AiPlanSuggestions { get; set; } = [];
    public ICollection<ActivityLog> ActivityLogs { get; set; } = [];
    public ICollection<GitHubConnection> GitHubConnections { get; set; } = [];
}
