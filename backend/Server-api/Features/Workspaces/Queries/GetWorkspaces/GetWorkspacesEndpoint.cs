namespace Server_api.Features.Workspaces.Queries.GetWorkspaces;

public sealed class GetWorkspacesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/workspaces", async (
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetWorkspacesQuery(), cancellationToken);
            return Results.Ok(result.Value);
        })
        .RequireAuthorization()
        .WithTags("Workspaces");
    }
}
