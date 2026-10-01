namespace Server_api.Features.Workspaces.Commands.UpdateWorkspace;

public sealed class UpdateWorkspaceEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/workspaces/{id:guid}", async (Guid id, UpdateWorkspaceRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new UpdateWorkspaceCommand(id, request.Name), cancellationToken);
            return result.Failure ? Results.NotFound(new { error = result.Error }) : Results.Ok(result.Value);
        }).RequireAuthorization().WithTags("Workspaces");
    }
}

public sealed record UpdateWorkspaceRequest(string Name);
