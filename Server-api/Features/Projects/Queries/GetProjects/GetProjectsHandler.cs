using MediatR.RequestHandling;
using Microsoft.EntityFrameworkCore;
using Server_api.Data;

namespace Server_api.Features.Projects.GetProjects;

public sealed class GetProjectsHandler(AppDbContext dbContext)
    : IQueryHandler<GetProjectsQuery, IReadOnlyList<ProjectListItem>>
{
    public async Task<MediatR.Result<IReadOnlyList<ProjectListItem>>> Handle(
        GetProjectsQuery query,
        CancellationToken cancellationToken)
    {
        var projects = await dbContext.Projects
            .AsNoTracking()
            .Where(x => query.WorkspaceId == null || x.WorkspaceId == query.WorkspaceId)
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
