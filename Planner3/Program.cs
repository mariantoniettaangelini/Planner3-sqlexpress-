using Microsoft.EntityFrameworkCore;
using Planner3.Data.DATACONTEXT;
using Planner3.Data.REPO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurazione DbContext
builder.Services.AddDbContext<PlannerContext>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IExerciseRepo, ExerciseRepo>();
builder.Services.AddScoped<IWorkoutSessionRepo, WorkoutSessionRepo>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
