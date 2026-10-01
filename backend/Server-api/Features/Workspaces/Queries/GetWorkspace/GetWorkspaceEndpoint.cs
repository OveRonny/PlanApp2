namespace Server_api.Features.Workspaces.Queries.GetWorkspace;

public sealed class GetWorkspaceEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/workspaces/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetWorkspaceQuery(id), cancellationToken);
            return result.Failure ? Results.NotFound(new { error = result.Error }) : Results.Ok(result.Value);
        }).RequireAuthorization().WithTags("Workspaces");
    }
}
