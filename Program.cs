using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(opt => opt.UseSqlite("Data Source=todoapi.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader();
	});
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

	app.Lifetime.ApplicationStarted.Register(() =>
	{
		var url = "http://localhost:5246/swagger";
		var psi = new System.Diagnostics.ProcessStartInfo
		{
			FileName = url,
			UseShellExecute = true
		};
		System.Diagnostics.Process.Start(psi);
	});
}
else
{
	app.UseHttpsRedirection();
}

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
