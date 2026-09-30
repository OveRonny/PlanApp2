namespace Server_api.Models;

public class WorkItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Title { get; set; }
    public string? Description { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? FeatureId { get; set; }
    public Guid? SprintId { get; set; }
    public Guid? ParentId { get; set; }
    public Guid? AssigneeId { get; set; }
    public WorkItemType Type { get; set; } = WorkItemType.Task;
    public WorkItemStatus Status { get; set; } = WorkItemStatus.Backlog;
    public WorkItemPriority Priority { get; set; } = WorkItemPriority.Medium;
    public int? StoryPoints { get; set; }
    public AiSource AiSource { get; set; } = AiSource.Manual;
    public string? AiContext { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public Project Project { get; set; } = null!;
    public Feature? Feature { get; set; }
    public Sprint? Sprint { get; set; }
    public User? Assignee { get; set; }
    public WorkItem? Parent { get; set; }
    public ICollection<WorkItem> SubItems { get; set; } = [];
    public ICollection<ActivityLog> ActivityLogs { get; set; } = [];
}
