using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkFlow.Api.Data;
using WorkFlow.Api.DTOs;
using WorkFlow.Api.Entities;

namespace WorkFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProjectController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> CreateProject(
        CreateProjectDto dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            OwnerId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);

        await _context.SaveChangesAsync();

        var projectDto = new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt,
            OwnerId = project.OwnerId
        };

        return CreatedAtAction(
            nameof(GetProject),
            new { id = project.Id },
            projectDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var projects = await _context.Projects
            .Where(p => p.OwnerId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt,
                OwnerId = p.OwnerId
            })
            .ToListAsync();

        return Ok(projects);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProjectDto>> GetProject(Guid id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var project = await _context.Projects
            .Where(p => p.Id == id && p.OwnerId == userId)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt,
                OwnerId = p.OwnerId
            })
            .FirstOrDefaultAsync();

        if (project is null)
        {
            return NotFound();
        }

        return Ok(project);
    }
}