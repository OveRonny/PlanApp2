namespace Server_api.Features.Projects.DeleteProject;

public sealed class DeleteProjectEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("/api/projects/{id:guid}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(
                new DeleteProjectCommand(id),
                cancellationToken);

            if (result.Failure)
            {
                return Results.NotFound(new { error = result.Error });
            }

            return Results.NoContent();
        }).WithTags("Projects");
    }
}
