namespace ToDoAPI.Classes;

public class Person
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public string? Email { get; set; }

	// Foreign key
	public int ProjectId { get; set; }

	// Navigation properties
	public Project Project { get; set; } = null!;
	public ICollection<Task> Tasks { get; set; } = [];
}
