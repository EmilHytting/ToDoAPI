using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Classes;
using ToDoAPI.Data;
using Task = ToDoAPI.Classes.Task;

namespace ToDoAPI.Controllers;

[ApiController]
[Route("api")]
public class TaskController : ControllerBase
{
	private readonly TodoDb _db;

	public TaskController(TodoDb db)
	{
		_db = db;
	}

	[HttpGet("projects/{projectId}/tasks")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<IEnumerable<TaskDTO>>> GetTasksByProject(int projectId)
	{
		var project = await _db.Projects.FindAsync(projectId);
		if (project is null)
			return NotFound(new { error = "Projekt ikke fundet" });

		var tasks = await _db.Tasks
			.Where(t => t.ProjectId == projectId)
			.Select(x => new TaskDTO(x))
			.ToArrayAsync();

		return Ok(tasks);
	}

	[HttpGet("tasks/{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<TaskDTO>> GetTask(int id)
	{
		var task = await _db.Tasks.FindAsync(id);
		if (task is null)
			return NotFound(new { error = "Task ikke fundet" });

		return Ok(new TaskDTO(task));
	}

	[HttpPost("projects/{projectId}/tasks")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<TaskDTO>> CreateTask(int projectId, TaskDTO dto)
	{
		var project = await _db.Projects.FindAsync(projectId);
		if (project is null)
			return NotFound(new { error = "Projekt ikke fundet" });

		if (string.IsNullOrWhiteSpace(dto.Title))
			return BadRequest(new { error = "Titel er påkrævet" });

		// Validér at AssignedToId er valid person hvis angivet
		if (dto.AssignedToId.HasValue)
		{
			var person = await _db.Person.FindAsync(dto.AssignedToId.Value);
			if (person is null || person.ProjectId != projectId)
				return BadRequest(new { error = "Personen eksisterer ikke i projektet" });
		}

		var task = new Task
		{
			Title = dto.Title,
			Description = dto.Description,
			Status = dto.Status,
			ProjectId = projectId,
			AssignedToId = dto.AssignedToId
		};

		_db.Tasks.Add(task);
		await _db.SaveChangesAsync();
		return CreatedAtAction(nameof(GetTask), new { id = task.Id }, new TaskDTO(task));
	}

	[HttpPut("tasks/{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> UpdateTask(int id, TaskDTO dto)
	{
		var task = await _db.Tasks.FindAsync(id);
		if (task is null)
			return NotFound(new { error = "Task ikke fundet" });

		if (string.IsNullOrWhiteSpace(dto.Title))
			return BadRequest(new { error = "Titel er påkrævet" });

		// Validér at AssignedToId er valid person hvis angivet
		if (dto.AssignedToId.HasValue)
		{
			var person = await _db.Person.FindAsync(dto.AssignedToId.Value);
			if (person is null || person.ProjectId != task.ProjectId)
				return BadRequest(new { error = "Personen eksisterer ikke i projektet" });
		}

		task.Title = dto.Title;
		task.Description = dto.Description;
		task.Status = dto.Status;
		task.AssignedToId = dto.AssignedToId;

		await _db.SaveChangesAsync();
		return NoContent();
	}

	[HttpDelete("tasks/{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> DeleteTask(int id)
	{
		var task = await _db.Tasks.FindAsync(id);
		if (task is null)
			return NotFound(new { error = "Task ikke fundet" });

		_db.Tasks.Remove(task);
		await _db.SaveChangesAsync();
		return NoContent();
	}
}
