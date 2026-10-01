namespace Server_api.Features.Workspaces.Queries.GetWorkspaces;

public sealed record GetWorkspacesQuery : IQuery<IReadOnlyList<WorkspaceListItem>>;

public sealed record WorkspaceListItem(
    Guid Id,
    string Name,
    Guid OwnerId,
    DateTime CreatedAt);
