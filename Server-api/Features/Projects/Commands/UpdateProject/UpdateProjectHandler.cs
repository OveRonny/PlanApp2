namespace Server_api.Features.Projects.UpdateProject;

public sealed class UpdateProjectHandler(AppDbContext dbContext)
    : ICommandHandler<UpdateProjectCommand, UpdateProjectResponse>
{
    public async Task<Result<UpdateProjectResponse>> Handle(
        UpdateProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await dbContext.Projects
            .SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (project is null)
        {
            return (Result<UpdateProjectResponse>)Result.Fail("Project was not found.");
        }

        project.Name = command.Name.Trim();
        project.Description = command.Description?.Trim();

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateProjectResponse(
            project.Id,
            project.WorkspaceId,
            project.Name,
            project.Description,
            project.CreatedAt);
    }
}
