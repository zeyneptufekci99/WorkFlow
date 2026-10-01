using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkFlow.Api.Data;
using WorkFlow.Api.DTOs;

namespace WorkFlow.Api.Controllers;

[ApiController]
[Route("api/projects/{projectId:guid}/members")]
[Authorize]
public class ProjectMemberController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProjectMemberController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectMemberDto>>> GetMembers(
        Guid projectId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var isMember = await _context.ProjectMembers
            .AnyAsync(pm =>
                pm.ProjectId == projectId &&
                pm.UserId == userId);

        if (!isMember)
        {
            return Forbid();
        }

        var members = await _context.ProjectMembers
            .Where(pm => pm.ProjectId == projectId)
            .Select(pm => new ProjectMemberDto
            {
                UserId = pm.UserId,
                FirstName = pm.User.FirstName,
                LastName = pm.User.LastName,
                Email = pm.User.Email,
                Role = pm.Role.ToString(),
                JoinedAt = pm.JoinedAt
            })
            .ToListAsync();

        return Ok(members);
    }
}