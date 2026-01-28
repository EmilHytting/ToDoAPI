namespace ToDoAPI.Classes;

public enum TaskStatus
{
	Pending = 0,
	InProgress = 1,
	Completed = 2
}

public class Task
{
	public int Id { get; set; }
	public required string Title { get; set; }
	public string? Description { get; set; }
	public TaskStatus Status { get; set; } = TaskStatus.Pending;

	// Foreign keys
	public int ProjectId { get; set; }
	public int? AssignedToId { get; set; }

	// Navigation properties
	public Project Project { get; set; } = null!;
	public Person? AssignedTo { get; set; }
	public ICollection<Todo> Todos { get; set; } = [];
}
