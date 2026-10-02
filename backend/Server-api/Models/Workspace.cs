namespace Server_api.Models;

public class Workspace
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public Guid OwnerId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User Owner { get; set; } = null!;
    public ICollection<Project> Projects { get; set; } = [];
    public ICollection<WorkspaceMember> Members { get; set; } = [];
}
