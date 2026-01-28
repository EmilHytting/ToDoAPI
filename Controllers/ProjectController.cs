using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Classes;
using ToDoAPI.Data;

namespace ToDoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
	private readonly TodoDb _db;

	public ProjectController(TodoDb db)
	{
		_db = db;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetAllProjects()
	{
		var projects = await _db.Projects.Select(x => new ProjectDTO(x)).ToArrayAsync();
		return Ok(projects);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<ProjectDTO>> GetProject(int id)
	{
		var project = await _db.Projects.FindAsync(id);
		if (project is null)
			return NotFound(new { error = "Projekt ikke fundet" });

		return Ok(new ProjectDTO(project));
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<ProjectDTO>> CreateProject(ProjectDTO dto)
	{
		if (string.IsNullOrWhiteSpace(dto.Name))
			return BadRequest(new { error = "Projektnavn er påkrævet" });

		var project = new Project { Name = dto.Name, Description = dto.Description };
		_db.Projects.Add(project);
		await _db.SaveChangesAsync();
		return CreatedAtAction(nameof(GetProject), new { id = project.Id }, new ProjectDTO(project));
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> UpdateProject(int id, ProjectDTO dto)
	{
		var project = await _db.Projects.FindAsync(id);
		if (project is null)
			return NotFound(new { error = "Projekt ikke fundet" });

		if (string.IsNullOrWhiteSpace(dto.Name))
			return BadRequest(new { error = "Projektnavn er påkrævet" });

		project.Name = dto.Name;
		project.Description = dto.Description;
		await _db.SaveChangesAsync();
		return NoContent();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> DeleteProject(int id)
	{
		var project = await _db.Projects.FindAsync(id);
		if (project is null)
			return NotFound(new { error = "Projekt ikke fundet" });

		_db.Projects.Remove(project);
		await _db.SaveChangesAsync();
		return NoContent();
	}
}
