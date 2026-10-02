namespace Server_api.Models;
public class Technology { public Guid Id { get; set; } = Guid.NewGuid(); public required string Name { get; set; } public TechnologyCategory Category { get; set; } public bool IsActive { get; set; } = true; public ICollection<ProjectTechnology> Projects { get; set; } = []; }
