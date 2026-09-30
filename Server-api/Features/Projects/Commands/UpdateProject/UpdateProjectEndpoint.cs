namespace Server_api.Features.Projects.UpdateProject;

public sealed class UpdateProjectEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/projects/{id:guid}", async (
            Guid id,
            UpdateProjectRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateProjectCommand(
                id,
                request.Name,
                request.Description);

            var result = await sender.Send(command, cancellationToken);

            if (result.Failure)
            {
                return Results.NotFound(new { error = result.Error });
            }

            var project = result.Value!;
            return Results.Ok(project);
        });
    }
}

public sealed record UpdateProjectRequest(
    string Name,
    string? Description);
