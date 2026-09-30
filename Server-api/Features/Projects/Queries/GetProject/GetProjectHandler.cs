using MediatR;
using MediatR.RequestHandling;
using Microsoft.EntityFrameworkCore;
using Server_api.Data;

namespace Server_api.Features.Projects.GetProject;

public sealed class GetProjectHandler(AppDbContext dbContext)
    : IQueryHandler<GetProjectQuery, GetProjectResponse>
{
    public async Task<Result<GetProjectResponse>> Handle(
        GetProjectQuery query,
        CancellationToken cancellationToken)
    {
        var project = await dbContext.Projects
            .AsNoTracking()
            .Where(x => x.Id == query.Id)
            .Select(x => new GetProjectResponse(
                x.Id,
                x.WorkspaceId,
                x.Name,
                x.Description,
                x.CreatedAt))
            .SingleOrDefaultAsync(cancellationToken);

        return project is null
            ? (Result<GetProjectResponse>)Result.Fail("Project was not found.")
            : project;
    }
}
