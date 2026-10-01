namespace Server_api.Features.Projects.GetProjects;

public sealed class GetProjectsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/projects", async (
            Guid? workspaceId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(
                new GetProjectsQuery(workspaceId),
                cancellationToken);

            return Results.Ok(result.Value);
        }).WithTags("Projects");
    }
}
