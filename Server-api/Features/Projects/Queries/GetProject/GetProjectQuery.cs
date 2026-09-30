namespace Server_api.Features.Projects.GetProject;

public sealed record GetProjectQuery(Guid Id) : IQuery<GetProjectResponse>;

public sealed record GetProjectResponse(
    Guid Id,
    Guid WorkspaceId,
    string Name,
    string? Description,
    DateTime CreatedAt);
