using System.Text.Json.Serialization;

namespace ToDoAPI.Classes;

public record TaskDTO
{
	public int Id { get; }
	public string Title { get; }
	public string? Description { get; }
	public TaskStatus Status { get; }
	public int ProjectId { get; }
	public int? AssignedToId { get; }

	[JsonConstructor]
	public TaskDTO(int id, string title, string? description, TaskStatus status, int projectId, int? assignedToId)
	{
		Id = id;
		Title = title;
		Description = description;
		Status = status;
		ProjectId = projectId;
		AssignedToId = assignedToId;
	}

	public TaskDTO(Task task) : this(task.Id, task.Title, task.Description, task.Status, task.ProjectId, task.AssignedToId) { }
}
