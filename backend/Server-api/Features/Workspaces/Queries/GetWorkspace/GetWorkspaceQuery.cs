namespace Server_api.Features.Workspaces.Queries.GetWorkspace;

public sealed record GetWorkspaceQuery(Guid Id) : IQuery<GetWorkspaceResponse>;

public sealed record GetWorkspaceResponse(Guid Id, string Name, Guid OwnerId, DateTime CreatedAt);
