using System.Text.Json.Serialization;

namespace ToDoAPI.Classes;

public record PersonDTO
{
	public int Id { get; }
	public string Name { get; }
	public string? Email { get; }
	public int ProjectId { get; }

	[JsonConstructor]
	public PersonDTO(int id, string name, string? email, int projectId)
	{
		Id = id;
		Name = name;
		Email = email;
		ProjectId = projectId;
	}

	public PersonDTO(Person person) : this(person.Id, person.Name, person.Email, person.ProjectId) { }
}
