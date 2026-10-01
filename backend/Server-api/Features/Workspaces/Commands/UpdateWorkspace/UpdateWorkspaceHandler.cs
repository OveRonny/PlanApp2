namespace Server_api.Features.Workspaces.Commands.UpdateWorkspace;

public sealed class UpdateWorkspaceHandler(AppDbContext dbContext, ICurrentUser currentUser)
    : ICommandHandler<UpdateWorkspaceCommand, UpdateWorkspaceResponse>
{
    public async Task<Result<UpdateWorkspaceResponse>> Handle(UpdateWorkspaceCommand command, CancellationToken cancellationToken)
    {
        var name = command.Name.Trim();
        var validationError = ValidationRules.NameError(name, "Workspace");
        if (validationError is not null)
            return (Result<UpdateWorkspaceResponse>)Result.Fail(validationError);

        var workspace = await dbContext.Workspaces.SingleOrDefaultAsync(
            x => x.Id == command.Id && x.OwnerId == currentUser.UserId, cancellationToken);

        if (workspace is null) return (Result<UpdateWorkspaceResponse>)Result.Fail("Workspace was not found.");

        workspace.Name = name;
        await dbContext.SaveChangesAsync(cancellationToken);
        return new UpdateWorkspaceResponse(workspace.Id, workspace.Name, workspace.OwnerId, workspace.CreatedAt);
    }
}
