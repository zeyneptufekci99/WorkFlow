namespace WorkFlow.Api.Entities;

public class ProjectMember
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }

    public Project Project { get; set; } = null!;

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public ProjectMemberRole Role { get; set; }

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}

public enum ProjectMemberRole
{
    Owner,
    Member
}