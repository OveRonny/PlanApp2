namespace Server_api.Models;

public class AiPlanSuggestion
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProjectId { get; set; }
    public Guid? CreatedById { get; set; }
    public required string Prompt { get; set; }
    public required string ProposedPlan { get; set; }
    public string? Provider { get; set; }
    public string? Model { get; set; }
    public bool IsApplied { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? AppliedAt { get; set; }
    public Project Project { get; set; } = null!;
    public User? CreatedBy { get; set; }
}
