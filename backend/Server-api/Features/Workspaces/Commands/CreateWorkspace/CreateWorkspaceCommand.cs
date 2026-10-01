namespace Server_api.Features.Workspaces.Commands.CreateWorkspace;

public sealed record CreateWorkspaceCommand(string Name) : ICommand<CreateWorkspaceResponse>;

public sealed record CreateWorkspaceResponse(
    Guid Id,
    string Name,
    Guid OwnerId,
    DateTime CreatedAt);
