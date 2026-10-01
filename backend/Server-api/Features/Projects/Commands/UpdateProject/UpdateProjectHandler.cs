namespace Server_api.Features.Projects.UpdateProject;

public sealed class UpdateProjectHandler(AppDbContext dbContext, ICurrentUser currentUser)
    : ICommandHandler<UpdateProjectCommand, UpdateProjectResponse>
{
    public async Task<Result<UpdateProjectResponse>> Handle(
        UpdateProjectCommand command,
        CancellationToken cancellationToken)
    {
        var name = command.Name.Trim();
        var nameError = ValidationRules.NameError(name, "Project");
        if (nameError is not null)
            return (Result<UpdateProjectResponse>)Result.Fail(nameError);
        var descriptionError = ValidationRules.DescriptionError(command.Description, "Project");
        if (descriptionError is not null)
            return (Result<UpdateProjectResponse>)Result.Fail(descriptionError);

        var project = await dbContext.Projects
            .SingleOrDefaultAsync(x => x.Id == command.Id &&
                (x.Workspace.OwnerId == currentUser.UserId || x.Members.Any(m => m.UserId == currentUser.UserId)), cancellationToken);

        if (project is null)
        {
            return (Result<UpdateProjectResponse>)Result.Fail("Project was not found.");
        }

        project.Name = name;
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
