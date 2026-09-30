using MediatR.RequestHandling;
using Server_api.Infrastructure.Endpoints;

namespace Server_api.Features.Projects.GetProject;

public sealed class GetProjectEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/projects/{id:guid}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetProjectQuery(id), cancellationToken);

            if (result.Failure)
            {
                return Results.NotFound(new { error = result.Error });
            }

            return Results.Ok(result.Value!);
        });
    }
}
