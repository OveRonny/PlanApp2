namespace Server_api.Models;

public class WorkspaceMember
{
    public Guid WorkspaceId { get; set; }
    public Guid GitHubUserId { get; set; }
    public WorkspaceRole Role { get; set; } = WorkspaceRole.Member;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public Workspace Workspace { get; set; } = null!;
    public GitHubUser GitHubUser { get; set; } = null!;
}
