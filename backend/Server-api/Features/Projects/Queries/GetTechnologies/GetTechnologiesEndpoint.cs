namespace Server_api.Features.Projects.Queries.GetTechnologies;
public sealed class GetTechnologiesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/technologies", (AppDbContext db, CancellationToken cancellationToken) => db.Technologies.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.Category).ThenBy(x => x.Name).ToListAsync(cancellationToken)).RequireAuthorization().WithTags("Projects");
    }
}
