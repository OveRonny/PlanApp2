namespace Server_api.Features.Projects.GetProjects;

public sealed record GetProjectsQuery(Guid? WorkspaceId) : IQuery<IReadOnlyList<ProjectListItem>>;

public sealed record ProjectListItem(
    Guid Id,
    Guid WorkspaceId,
    string Name,
    string? Description,
    DateTime CreatedAt);
