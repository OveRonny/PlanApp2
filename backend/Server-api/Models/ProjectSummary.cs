namespace Server_api.Models;

public class ProjectSummary
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProjectId { get; set; }
    public required string Content { get; set; }
    public string? Highlights { get; set; }
    public string? Risks { get; set; }
    public string? NextSteps { get; set; }
    public AiSource Source { get; set; } = AiSource.AiGenerated;
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public Project Project { get; set; } = null!;
}
