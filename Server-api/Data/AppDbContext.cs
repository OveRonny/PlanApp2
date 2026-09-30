namespace Server_api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Workspace> Workspaces => Set<Workspace>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
    public DbSet<Sprint> Sprints => Set<Sprint>();
    public DbSet<Feature> Features => Set<Feature>();
    public DbSet<WorkItem> WorkItems => Set<WorkItem>();
    public DbSet<AiPlanSuggestion> AiPlanSuggestions => Set<AiPlanSuggestion>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
    public DbSet<ProjectSummary> ProjectSummaries => Set<ProjectSummary>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();

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
