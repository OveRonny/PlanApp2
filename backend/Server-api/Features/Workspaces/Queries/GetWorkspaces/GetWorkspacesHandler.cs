namespace Server_api.Features.Workspaces.Queries.GetWorkspaces;

public sealed class GetWorkspacesHandler(
    AppDbContext dbContext,
    ICurrentUser currentUser)
    : IQueryHandler<GetWorkspacesQuery, IReadOnlyList<WorkspaceListItem>>
{
    public async Task<Result<IReadOnlyList<WorkspaceListItem>>> Handle(
        GetWorkspacesQuery query,
        CancellationToken cancellationToken)
    {
        var workspaces = await dbContext.Workspaces
            .AsNoTracking()
            .Where(x => x.OwnerId == currentUser.UserId)
            .OrderBy(x => x.Name)
            .Select(x => new WorkspaceListItem(x.Id, x.Name, x.OwnerId, x.CreatedAt))
            .ToListAsync(cancellationToken);

        return workspaces;
    }
}
