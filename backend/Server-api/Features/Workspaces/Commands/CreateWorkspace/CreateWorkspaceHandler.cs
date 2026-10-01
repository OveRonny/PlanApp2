namespace Server_api.Features.Workspaces.Commands.CreateWorkspace;

public sealed class CreateWorkspaceHandler(
    AppDbContext dbContext,
    ICurrentUser currentUser)
    : ICommandHandler<CreateWorkspaceCommand, CreateWorkspaceResponse>
{
    public async Task<Result<CreateWorkspaceResponse>> Handle(
        CreateWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var name = command.Name.Trim();
        var validationError = ValidationRules.NameError(name, "Workspace");
        if (validationError is not null)
            return (Result<CreateWorkspaceResponse>)Result.Fail(validationError);

        var workspace = new Workspace
        {
            Name = name,
            OwnerId = currentUser.UserId
        };

        dbContext.Workspaces.Add(workspace);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateWorkspaceResponse(
            workspace.Id,
            workspace.Name,
            workspace.OwnerId,
            workspace.CreatedAt);
    }
}
