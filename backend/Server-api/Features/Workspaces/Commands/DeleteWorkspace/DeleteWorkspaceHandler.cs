namespace Server_api.Features.Workspaces.Commands.DeleteWorkspace;

public sealed class DeleteWorkspaceHandler(AppDbContext dbContext, ICurrentUser currentUser)
    : ICommandHandler<DeleteWorkspaceCommand, DeleteWorkspaceResponse>
{
    public async Task<Result<DeleteWorkspaceResponse>> Handle(DeleteWorkspaceCommand command, CancellationToken cancellationToken)
    {
        var workspace = await dbContext.Workspaces.SingleOrDefaultAsync(
            x => x.Id == command.Id && x.OwnerId == currentUser.UserId, cancellationToken);

        if (workspace is null) return (Result<DeleteWorkspaceResponse>)Result.Fail("Workspace was not found.");

        dbContext.Workspaces.Remove(workspace);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteWorkspaceResponse(workspace.Id);
    }
}
