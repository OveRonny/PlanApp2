namespace Server_api.Features.Projects.GetProjects;

public sealed class GetProjectsHandler(AppDbContext dbContext, ICurrentUser currentUser)
    : IQueryHandler<GetProjectsQuery, IReadOnlyList<ProjectListItem>>
{
    public async Task<MediatR.Result<IReadOnlyList<ProjectListItem>>> Handle(
        GetProjectsQuery query,
        CancellationToken cancellationToken)
    {
        var projects = await dbContext.Projects
            .AsNoTracking()
            .Where(x => (query.WorkspaceId == null || x.WorkspaceId == query.WorkspaceId) &&
                (x.Workspace.OwnerId == currentUser.UserId || x.Members.Any(m => m.UserId == currentUser.UserId)))
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new ProjectListItem(
                x.Id,
                x.WorkspaceId,
                x.Name,
                x.Description,
                x.CreatedAt))
            .ToListAsync(cancellationToken);

        return projects;
    }
}
