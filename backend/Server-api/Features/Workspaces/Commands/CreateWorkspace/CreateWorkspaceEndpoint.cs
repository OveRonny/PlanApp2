namespace Server_api.Features.Workspaces.Commands.CreateWorkspace;

public sealed class CreateWorkspaceEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/workspaces", async (
            CreateWorkspaceRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(
                new CreateWorkspaceCommand(request.Name),
                cancellationToken);

            return Results.Created($"/api/workspaces/{result.Value!.Id}", result.Value);
        })
        .RequireAuthorization()
        .WithTags("Workspaces");
    }
}

public sealed record CreateWorkspaceRequest(string Name);
