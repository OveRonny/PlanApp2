namespace Server_api.Models;

public class Feature
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Title { get; set; }
    public string? Description { get; set; }
    public Guid ProjectId { get; set; }
    public WorkItemStatus Status { get; set; } = WorkItemStatus.Backlog;
    public AiSource AiSource { get; set; } = AiSource.Manual;
    public string? AiContext { get; set; }
    public Project Project { get; set; } = null!;
    public ICollection<WorkItem> WorkItems { get; set; } = [];
    public ICollection<ActivityLog> ActivityLogs { get; set; } = [];
}
