namespace ToDoAPI.Classes;

public class Todo
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public bool IsComplete { get; set; }

	// Foreign key
	public int? TaskId { get; set; }

	// Navigation property
	public Task? Task { get; set; }
}
