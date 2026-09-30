using MediatR.RequestHandling;

namespace Server_api.Features.Projects.CreateProject;

public sealed record CreateProjectCommand(
    Guid WorkspaceId,
    string Name,
    string? Description) : ICommand<CreateProjectResponse>;

public sealed record CreateProjectResponse(
    Guid Id,
    Guid WorkspaceId,
    string Name,
    string? Description,
    DateTime CreatedAt);
