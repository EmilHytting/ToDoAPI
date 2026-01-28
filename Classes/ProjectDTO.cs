namespace ToDoAPI.Classes;

using System.Text.Json.Serialization;

public record ProjectDTO
{
    public int Id { get; }
    public string Name { get; }
    public string? Description { get; }

    [JsonConstructor]
    public ProjectDTO(int id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public ProjectDTO(Project project) : this(project.Id, project.Name, project.Description) { }
}

