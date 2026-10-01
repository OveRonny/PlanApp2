namespace Server_api.Features.Projects.CreateProject;

public sealed class CreateProjectEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/projects", async (
            CreateProjectRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateProjectCommand(
                request.WorkspaceId,
                request.Name,
                request.Description);

            var result = await sender.Send(command, cancellationToken);

            if (result.Failure)
            {
                return Results.BadRequest(new { error = result.Error });
            }

            var project = result.Value!;
            return Results.Created($"/api/projects/{project.Id}", project);
        }).WithTags("Projects");

    }
}

public sealed record CreateProjectRequest(
    Guid WorkspaceId,
    string Name,
    string? Description);
