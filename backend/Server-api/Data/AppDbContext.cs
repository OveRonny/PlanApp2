namespace Server_api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<
        User,
        IdentityRole<Guid>,
        Guid,
        IdentityUserClaim<Guid>,
        IdentityUserRole<Guid>,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>(options)
{
    public DbSet<Workspace> Workspaces => Set<Workspace>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
    public DbSet<Sprint> Sprints => Set<Sprint>();
    public DbSet<Feature> Features => Set<Feature>();
    public DbSet<WorkItem> WorkItems => Set<WorkItem>();
    public DbSet<AiPlanSuggestion> AiPlanSuggestions => Set<AiPlanSuggestion>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
    public DbSet<ProjectSummary> ProjectSummaries => Set<ProjectSummary>();
    public DbSet<GitHubConnection> GitHubConnections => Set<GitHubConnection>();
    public DbSet<GitHubUser> GitHubUsers => Set<GitHubUser>();
    public DbSet<WorkspaceMember> WorkspaceMembers => Set<WorkspaceMember>();
    public DbSet<ProjectRepository> ProjectRepositories => Set<ProjectRepository>();
    public DbSet<Technology> Technologies => Set<Technology>();
    public DbSet<ProjectTechnology> ProjectTechnologies => Set<ProjectTechnology>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Technology>().HasIndex(x => new { x.Name, x.Category }).IsUnique();
        modelBuilder.Entity<ProjectTechnology>().HasKey(x => new { x.ProjectId, x.TechnologyId });
        modelBuilder.Entity<ProjectTechnology>().HasOne(x => x.Project).WithMany(x => x.Technologies).HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<ProjectTechnology>().HasOne(x => x.Technology).WithMany(x => x.Projects).HasForeignKey(x => x.TechnologyId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Technology>().Property(x => x.Name).HasMaxLength(100);
        modelBuilder.Entity<Technology>().HasData(
            new { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Name = "Vue", Category = TechnologyCategory.Frontend, IsActive = true },
            new { Id = Guid.Parse("10000000-0000-0000-0000-000000000002"), Name = "React", Category = TechnologyCategory.Frontend, IsActive = true },
            new { Id = Guid.Parse("10000000-0000-0000-0000-000000000003"), Name = "TypeScript", Category = TechnologyCategory.Frontend, IsActive = true },
            new { Id = Guid.Parse("10000000-0000-0000-0000-000000000004"), Name = "ASP.NET Core", Category = TechnologyCategory.Backend, IsActive = true },
            new { Id = Guid.Parse("10000000-0000-0000-0000-000000000005"), Name = "Node.js", Category = TechnologyCategory.Backend, IsActive = true },
            new { Id = Guid.Parse("10000000-0000-0000-0000-000000000006"), Name = "SQL Server", Category = TechnologyCategory.Database, IsActive = true },
            new { Id = Guid.Parse("10000000-0000-0000-0000-000000000007"), Name = "PostgreSQL", Category = TechnologyCategory.Database, IsActive = true },
            new { Id = Guid.Parse("10000000-0000-0000-0000-000000000008"), Name = "Docker", Category = TechnologyCategory.Infrastructure, IsActive = true },
            new { Id = Guid.Parse("10000000-0000-0000-0000-000000000009"), Name = "GitHub Actions", Category = TechnologyCategory.Infrastructure, IsActive = true },
            new { Id = Guid.Parse("10000000-0000-0000-0000-000000000010"), Name = "xUnit", Category = TechnologyCategory.Testing, IsActive = true },
            new { Id = Guid.Parse("10000000-0000-0000-0000-000000000011"), Name = "Vitest", Category = TechnologyCategory.Testing, IsActive = true },
            new { Id = Guid.Parse("10000000-0000-0000-0000-000000000012"), Name = "REST", Category = TechnologyCategory.Integration, IsActive = true },
            new { Id = Guid.Parse("10000000-0000-0000-0000-000000000013"), Name = "GraphQL", Category = TechnologyCategory.Integration, IsActive = true },
            new { Id = Guid.Parse("10000000-0000-0000-0000-000000000014"), Name = "Git", Category = TechnologyCategory.Tooling, IsActive = true });

        modelBuilder.Entity<GitHubConnection>().HasIndex(x => new { x.UserId, x.GitHubUserId }).IsUnique();
        modelBuilder.Entity<GitHubUser>().HasIndex(x => x.GitHubId).IsUnique();
        modelBuilder.Entity<WorkspaceMember>().HasKey(x => new { x.WorkspaceId, x.GitHubUserId });
        modelBuilder.Entity<ProjectRepository>().HasIndex(x => new { x.ProjectId, x.GitHubRepositoryId }).IsUnique();

        modelBuilder.Entity<GitHubConnection>().Property(x => x.GitHubUserId).HasMaxLength(100);
        modelBuilder.Entity<GitHubConnection>().Property(x => x.GitHubLogin).HasMaxLength(100);
        modelBuilder.Entity<GitHubUser>().Property(x => x.GitHubId).HasMaxLength(100);
        modelBuilder.Entity<GitHubUser>().Property(x => x.Login).HasMaxLength(100);
        modelBuilder.Entity<ProjectRepository>().Property(x => x.GitHubRepositoryId).HasMaxLength(100);
        modelBuilder.Entity<ProjectRepository>().Property(x => x.FullName).HasMaxLength(250);

        modelBuilder.Entity<GitHubConnection>()
            .HasOne(x => x.User).WithMany(x => x.GitHubConnections)
            .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WorkspaceMember>()
            .HasOne(x => x.Workspace).WithMany(x => x.Members)
            .HasForeignKey(x => x.WorkspaceId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WorkspaceMember>()
            .HasOne(x => x.GitHubUser).WithMany(x => x.WorkspaceMemberships)
            .HasForeignKey(x => x.GitHubUserId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProjectRepository>()
            .HasOne(x => x.Project).WithOne(x => x.GitHubRepository)
            .HasForeignKey<ProjectRepository>(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProjectMember>()
            .HasKey(x => new { x.ProjectId, x.UserId });

        modelBuilder.Entity<ProjectMember>()
            .HasOne(x => x.Project).WithMany(x => x.Members)
            .HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProjectMember>()
            .HasOne(x => x.User).WithMany(x => x.ProjectMemberships)
            .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Workspace>()
            .HasOne(x => x.Owner).WithMany(x => x.OwnedWorkspaces)
            .HasForeignKey(x => x.OwnerId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Project>()
            .HasOne(x => x.Workspace).WithMany(x => x.Projects)
            .HasForeignKey(x => x.WorkspaceId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Sprint>()
            .HasOne(x => x.Project).WithMany(x => x.Sprints)
            .HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Feature>()
            .HasOne(x => x.Project).WithMany(x => x.Features)
            .HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WorkItem>()
            .HasOne(x => x.Project).WithMany(x => x.WorkItems)
            .HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WorkItem>()
            .HasOne(x => x.Feature).WithMany(x => x.WorkItems)
            .HasForeignKey(x => x.FeatureId).OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<WorkItem>()
            .HasOne(x => x.Sprint).WithMany(x => x.WorkItems)
            .HasForeignKey(x => x.SprintId).OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<WorkItem>()
            .HasOne(x => x.Assignee).WithMany(x => x.AssignedWorkItems)
            .HasForeignKey(x => x.AssigneeId).OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<WorkItem>()
            .HasOne(x => x.Parent).WithMany(x => x.SubItems)
            .HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AiPlanSuggestion>().HasOne(x => x.Project).WithMany(x => x.AiPlanSuggestions).HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<AiPlanSuggestion>().HasOne(x => x.Feature).WithMany().HasForeignKey(x => x.FeatureId).OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<AiPlanSuggestion>().HasOne(x => x.CreatedBy).WithMany(x => x.AiPlanSuggestions).HasForeignKey(x => x.CreatedById).OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<ActivityLog>().HasOne(x => x.Project).WithMany(x => x.ActivityLogs).HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<ActivityLog>().HasOne(x => x.User).WithMany(x => x.ActivityLogs).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<ActivityLog>().HasOne(x => x.WorkItem).WithMany(x => x.ActivityLogs).HasForeignKey(x => x.WorkItemId).OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<ActivityLog>().HasOne(x => x.Feature).WithMany(x => x.ActivityLogs).HasForeignKey(x => x.FeatureId).OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<ProjectSummary>().HasOne(x => x.Project).WithMany(x => x.Summaries).HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>().Property(x => x.Email).HasMaxLength(320);
        modelBuilder.Entity<Workspace>().Property(x => x.Name).HasMaxLength(150);
        modelBuilder.Entity<Project>().Property(x => x.Name).HasMaxLength(150);
        modelBuilder.Entity<Feature>().Property(x => x.Title).HasMaxLength(250);
        modelBuilder.Entity<WorkItem>().Property(x => x.Title).HasMaxLength(250);
        modelBuilder.Entity<ActivityLog>().Property(x => x.Action).HasMaxLength(100);
    }
}
