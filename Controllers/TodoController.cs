using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Classes;
using ToDoAPI.Data;

namespace ToDoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
	private readonly TodoDb _db;

	public TodoController(TodoDb db)
	{
		_db = db;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetAllTodos()
	{
		var todos = await _db.Todos.Select(x => new TodoItemDTO(x)).ToArrayAsync();
		return Ok(todos);
	}

	[HttpGet("complete")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetCompleteTodos()
	{
		var todos = await _db.Todos.Where(t => t.IsComplete).Select(x => new TodoItemDTO(x)).ToListAsync();
		return Ok(todos);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<TodoItemDTO>> GetTodo(int id)
	{
		var todo = await _db.Todos.FindAsync(id);
		if (todo is null)
			return NotFound(new { error = "Todo ikke fundet" });

		return Ok(new TodoItemDTO(todo));
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<TodoItemDTO>> CreateTodo(TodoItemDTO dto)
	{
		if (string.IsNullOrWhiteSpace(dto.Name))
			return BadRequest(new { error = "Navn er påkrævet" });

		var todo = new Todo { IsComplete = dto.IsComplete, Name = dto.Name };
		_db.Todos.Add(todo);
		await _db.SaveChangesAsync();
		return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, new TodoItemDTO(todo));
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> UpdateTodo(int id, TodoItemDTO dto)
	{
		var todo = await _db.Todos.FindAsync(id);
		if (todo is null)
			return NotFound(new { error = "Todo ikke fundet" });

		if (string.IsNullOrWhiteSpace(dto.Name))
			return BadRequest(new { error = "Navn er påkrævet" });

		todo.Name = dto.Name;
		todo.IsComplete = dto.IsComplete;
		await _db.SaveChangesAsync();
		return NoContent();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> DeleteTodo(int id)
	{
		var todo = await _db.Todos.FindAsync(id);
		if (todo is null)
			return NotFound(new { error = "Todo ikke fundet" });

		_db.Todos.Remove(todo);
		await _db.SaveChangesAsync();
		return NoContent();
	}
}
