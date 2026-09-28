namespace WorkFlow.Api.Entities;

public class Project
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Guid OwnerId { get; set; }

    public User Owner { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ProjectMember> Members { get; set; } = [];
}