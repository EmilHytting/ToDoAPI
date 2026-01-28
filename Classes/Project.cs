namespace ToDoAPI.Classes;

public class Project
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public string? Description { get; set; }
	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

	// Navigation properties
	public ICollection<Person> Person { get; set; } = [];
	public ICollection<Task> Tasks { get; set; } = [];
}
