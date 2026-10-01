namespace Server_api.Features.Workspaces.Queries.GetWorkspace;

public sealed class GetWorkspaceHandler(AppDbContext dbContext, ICurrentUser currentUser)
    : IQueryHandler<GetWorkspaceQuery, GetWorkspaceResponse>
{
    public async Task<Result<GetWorkspaceResponse>> Handle(GetWorkspaceQuery query, CancellationToken cancellationToken)
    {
        var workspace = await dbContext.Workspaces.AsNoTracking()
            .Where(x => x.Id == query.Id && x.OwnerId == currentUser.UserId)
            .Select(x => new GetWorkspaceResponse(x.Id, x.Name, x.OwnerId, x.CreatedAt))
            .SingleOrDefaultAsync(cancellationToken);

        return workspace is null
            ? (Result<GetWorkspaceResponse>)Result.Fail("Workspace was not found.")
            : workspace;
    }
}
