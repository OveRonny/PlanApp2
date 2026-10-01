namespace Server_api.Features.Projects.UpdateProject;

public sealed record UpdateProjectCommand(
    Guid Id,
    string Name,
    string? Description) : ICommand<UpdateProjectResponse>;

public sealed record UpdateProjectResponse(
    Guid Id,
    Guid WorkspaceId,
    string Name,
    string? Description,
    DateTime CreatedAt);
