namespace Server_api.Features.Workspaces.Commands.UpdateWorkspace;

public sealed class UpdateWorkspaceHandler(AppDbContext dbContext, ICurrentUser currentUser)
    : ICommandHandler<UpdateWorkspaceCommand, UpdateWorkspaceResponse>
{
    public async Task<Result<UpdateWorkspaceResponse>> Handle(UpdateWorkspaceCommand command, CancellationToken cancellationToken)
    {
        var workspace = await dbContext.Workspaces.SingleOrDefaultAsync(
            x => x.Id == command.Id && x.OwnerId == currentUser.UserId, cancellationToken);

        if (workspace is null) return (Result<UpdateWorkspaceResponse>)Result.Fail("Workspace was not found.");

        workspace.Name = command.Name.Trim();
        await dbContext.SaveChangesAsync(cancellationToken);
        return new UpdateWorkspaceResponse(workspace.Id, workspace.Name, workspace.OwnerId, workspace.CreatedAt);
    }
}
