namespace Server_api.Features.Projects.CreateProject;

public sealed class CreateProjectHandler(AppDbContext dbContext, ICurrentUser currentUser)
    : ICommandHandler<CreateProjectCommand, CreateProjectResponse>
{
    public async Task<Result<CreateProjectResponse>> Handle(
        CreateProjectCommand command,
        CancellationToken cancellationToken)
    {
        var name = command.Name.Trim();
        var nameError = ValidationRules.NameError(name, "Project");
        if (nameError is not null)
            return (Result<CreateProjectResponse>)Result.Fail(nameError);
        var descriptionError = ValidationRules.DescriptionError(command.Description, "Project");
        if (descriptionError is not null)
            return (Result<CreateProjectResponse>)Result.Fail(descriptionError);

        var workspaceExists = await dbContext.Workspaces.AnyAsync(x =>
            x.Id == command.WorkspaceId &&
            (x.OwnerId == currentUser.UserId || x.Projects.Any(p => p.Members.Any(m => m.UserId == currentUser.UserId))), cancellationToken);

        if (!workspaceExists)
        {
            return (Result<CreateProjectResponse>)Result.Fail("Workspace was not found.");
        }

        var project = new Project
        {
            WorkspaceId = command.WorkspaceId,
            Name = name,
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
