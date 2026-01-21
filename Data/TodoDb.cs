using Microsoft.EntityFrameworkCore;
using ToDoAPI.Classes;

namespace ToDoAPI.Data
{
    public class TodoDb : DbContext
    {
        public TodoDb(DbContextOptions<TodoDb> options) : base(options)
        {
		}

        public DbSet<Todo> Todos => Set<Todo>();
	}
}
