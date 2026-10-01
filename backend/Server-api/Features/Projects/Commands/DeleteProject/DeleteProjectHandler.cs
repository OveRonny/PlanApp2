namespace Server_api.Features.Projects.DeleteProject;

public sealed class DeleteProjectHandler(AppDbContext dbContext)
    : ICommandHandler<DeleteProjectCommand, DeleteProjectResponse>
{
    public async Task<Result<DeleteProjectResponse>> Handle(
        DeleteProjectCommand command,
        CancellationToken cancellationToken)
    {
        var project = await dbContext.Projects
            .SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (project is null)
        {
            return (Result<DeleteProjectResponse>)Result.Fail("Project was not found.");
        }

        dbContext.Projects.Remove(project);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteProjectResponse(project.Id);
    }
}
