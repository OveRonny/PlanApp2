namespace Server_api.Features.Workspaces.Commands.DeleteWorkspace;

public sealed class DeleteWorkspaceEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("/api/workspaces/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new DeleteWorkspaceCommand(id), cancellationToken);
            return result.Failure ? Results.NotFound(new { error = result.Error }) : Results.NoContent();
        }).RequireAuthorization().WithTags("Workspaces");
    }
}
