namespace Server_api.Models;

public class ProjectMember
{
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
    public ProjectRole Role { get; set; } = ProjectRole.Member;
    public Project Project { get; set; } = null!;
    public User User { get; set; } = null!;
}
