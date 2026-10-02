namespace Server_api.Infrastructure.OpenAI;

public interface IOpenAiPlanner { Task<string> GenerateFeaturePlanAsync(string projectName, string projectDescription, IReadOnlyList<string> technologies, string request, CancellationToken cancellationToken); }
