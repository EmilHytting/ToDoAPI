using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Classes;
using ToDoAPI.Data;

namespace ToDoAPI.Controllers;

[ApiController]
[Route("api")]
public class PersonController : ControllerBase
{
	private readonly TodoDb _db;

	public PersonController(TodoDb db)
	{
		_db = db;
	}

	[HttpGet("projects/{projectId}/person")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<IEnumerable<PersonDTO>>> GetPersonsByProject(int projectId)
	{
		var project = await _db.Projects.FindAsync(projectId);
		if (project is null)
			return NotFound(new { error = "Projekt ikke fundet" });

		var persons = await _db.Person
			.Where(p => p.ProjectId == projectId)
			.Select(x => new PersonDTO(x))
			.ToArrayAsync();

		return Ok(persons);
	}

	[HttpGet("person/{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<PersonDTO>> GetPerson(int id)
	{
		var person = await _db.Person.FindAsync(id);
		if (person is null)
			return NotFound(new { error = "Person ikke fundet" });

		return Ok(new PersonDTO(person));
	}

	[HttpPost("projects/{projectId}/person")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<PersonDTO>> CreatePerson(int projectId, PersonDTO dto)
	{
		var project = await _db.Projects.FindAsync(projectId);
		if (project is null)
			return NotFound(new { error = "Projekt ikke fundet" });

		if (string.IsNullOrWhiteSpace(dto.Name))
			return BadRequest(new { error = "Navn er påkrævet" });

		var person = new Person
		{
			Name = dto.Name,
			Email = dto.Email,
			ProjectId = projectId
		};

		_db.Person.Add(person);
		await _db.SaveChangesAsync();
		return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, new PersonDTO(person));
	}

	[HttpPut("person/{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> UpdatePerson(int id, PersonDTO dto)
	{
		var person = await _db.Person.FindAsync(id);
		if (person is null)
			return NotFound(new { error = "Person ikke fundet" });

		if (string.IsNullOrWhiteSpace(dto.Name))
			return BadRequest(new { error = "Navn er påkrævet" });

		person.Name = dto.Name;
		person.Email = dto.Email;

		await _db.SaveChangesAsync();
		return NoContent();
	}

	[HttpDelete("person/{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> DeletePerson(int id)
	{
		var person = await _db.Person.FindAsync(id);
		if (person is null)
			return NotFound(new { error = "Person ikke fundet" });

		_db.Person.Remove(person);
		await _db.SaveChangesAsync();
		return NoContent();
	}
}
