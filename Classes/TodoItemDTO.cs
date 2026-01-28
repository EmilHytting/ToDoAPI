namespace ToDoAPI.Classes;

public record TodoItemDTO(int Id, string? Name, bool IsComplete, int? TaskId = null)
{
	public TodoItemDTO(Todo todoItem) : this(todoItem.Id, todoItem.Name, todoItem.IsComplete, todoItem.TaskId) { }
}
