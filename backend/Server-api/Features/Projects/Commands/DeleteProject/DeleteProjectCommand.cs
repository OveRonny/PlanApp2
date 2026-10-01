namespace Server_api.Features.Projects.DeleteProject;

public sealed record DeleteProjectCommand(Guid Id) : ICommand<DeleteProjectResponse>;

public sealed record DeleteProjectResponse(Guid Id);
