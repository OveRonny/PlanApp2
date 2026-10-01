namespace Server_api.Features.Projects.CreateProject;

public sealed class CreateProjectHandler(AppDbContext dbContext)
    : ICommandHandler<CreateProjectCommand, CreateProjectResponse>
{
    public async Task<Result<CreateProjectResponse>> Handle(
        CreateProjectCommand command,
        CancellationToken cancellationToken)
    {
        var workspaceExists = await dbContext.Workspaces
            .AnyAsync(x => x.Id == command.WorkspaceId, cancellationToken);

        if (!workspaceExists)
        {
            return (Result<CreateProjectResponse>)Result.Fail("Workspace was not found.");
        }

        var project = new Project
        {
            WorkspaceId = command.WorkspaceId,
            Name = command.Name.Trim(),
            Description = command.Description?.Trim()
        };

        dbContext.Projects.Add(project);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateProjectResponse(
            project.Id,
            project.WorkspaceId,
            project.Name,
            project.Description,
            project.CreatedAt);
    }
}
