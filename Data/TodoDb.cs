using Microsoft.EntityFrameworkCore;
using ToDoAPI.Classes;
using Task = ToDoAPI.Classes.Task;

namespace ToDoAPI.Data;

public class TodoDb(DbContextOptions<TodoDb> options) : DbContext(options)
{
	public DbSet<Project> Projects => Set<Project>();
	public DbSet<Person> Person => Set<Person>();
	public DbSet<Task> Tasks => Set<Task>();
	public DbSet<Todo> Todos => Set<Todo>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Configure table names
		modelBuilder.Entity<Person>().ToTable("Person");

		// Project relationships
		modelBuilder.Entity<Project>()
			.HasMany(p => p.Person)
			.WithOne(pe => pe.Project)
			.HasForeignKey(pe => pe.ProjectId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<Project>()
			.HasMany(p => p.Tasks)
			.WithOne(t => t.Project)
			.HasForeignKey(t => t.ProjectId)
			.OnDelete(DeleteBehavior.Cascade);

		// Task relationships
		modelBuilder.Entity<Task>()
			.HasMany(t => t.Todos)
			.WithOne(to => to.Task)
			.HasForeignKey(to => to.TaskId)
			.OnDelete(DeleteBehavior.SetNull);

		modelBuilder.Entity<Task>()
			.HasOne(t => t.AssignedTo)
			.WithMany(p => p.Tasks)
			.HasForeignKey(t => t.AssignedToId)
			.OnDelete(DeleteBehavior.SetNull);
	}
}
