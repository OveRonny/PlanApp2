namespace Server_api.Models;

public class Sprint
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public string? Goal { get; set; }
    public Guid ProjectId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public bool IsCompleted { get; set; }
    public Project Project { get; set; } = null!;
    public ICollection<WorkItem> WorkItems { get; set; } = [];
    public ICollection<ActivityLog> ActivityLogs { get; set; } = [];
}
