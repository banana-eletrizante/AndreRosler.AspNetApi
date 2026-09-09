using AndreRosler.AspNetApi.Data;
using AndreRosler.AspNetApi.DTOs;
using AndreRosler.AspNetApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AndreRosler.AspNetApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _db;

    public TasksController(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>List all tasks (public).</summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<TaskResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TaskResponse>>> GetAll()
    {
        var items = await _db.Tasks
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TaskResponse(t.Id, t.Title, t.Description, t.IsDone, t.CreatedAt))
            .ToListAsync();

        return Ok(items);
    }

    /// <summary>Get a task by id (public).</summary>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponse>> GetById(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null)
        {
            return NotFound();
        }

        return Ok(new TaskResponse(task.Id, task.Title, task.Description, task.IsDone, task.CreatedAt));
    }

    /// <summary>Create a task (requires JWT).</summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TaskResponse>> Create([FromBody] TaskCreateRequest request)
    {
        var task = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            IsDone = request.IsDone,
            CreatedAt = DateTime.UtcNow
        };

        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();

        var response = new TaskResponse(task.Id, task.Title, task.Description, task.IsDone, task.CreatedAt);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, response);
    }

    /// <summary>Update a task (requires JWT).</summary>
    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TaskResponse>> Update(int id, [FromBody] TaskUpdateRequest request)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null)
        {
            return NotFound();
        }

        task.Title = request.Title;
        task.Description = request.Description;
        task.IsDone = request.IsDone;

        await _db.SaveChangesAsync();

        return Ok(new TaskResponse(task.Id, task.Title, task.Description, task.IsDone, task.CreatedAt));
    }

    /// <summary>Delete a task (requires JWT).</summary>
    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null)
        {
            return NotFound();
        }

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
