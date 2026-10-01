namespace Server_api.Models;

public class ActivityLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProjectId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? WorkItemId { get; set; }
    public Guid? FeatureId { get; set; }
    public required string Action { get; set; }
    public required string Description { get; set; }
    public string? ChangesJson { get; set; }
    public AiSource Source { get; set; } = AiSource.Manual;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Project Project { get; set; } = null!;
    public User? User { get; set; }
    public WorkItem? WorkItem { get; set; }
    public Feature? Feature { get; set; }
}
