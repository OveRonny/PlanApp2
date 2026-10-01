namespace Server_api.Features.Workspaces.Commands.UpdateWorkspace;

public sealed record UpdateWorkspaceCommand(Guid Id, string Name) : ICommand<UpdateWorkspaceResponse>;

public sealed record UpdateWorkspaceResponse(Guid Id, string Name, Guid OwnerId, DateTime CreatedAt);
