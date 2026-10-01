namespace Server_api.Features.Workspaces.Commands.DeleteWorkspace;

public sealed record DeleteWorkspaceCommand(Guid Id) : ICommand<DeleteWorkspaceResponse>;

public sealed record DeleteWorkspaceResponse(Guid Id);
