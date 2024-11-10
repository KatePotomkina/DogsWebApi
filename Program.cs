using DogsWebApi.Application.Interfaces;
using DogsWebApi.Application.Services;
using DogsWebApi.Infrastructure.Interfaces;
using DogsWebApi.Infrastructure.Persistence;
using DogsWebApi.Infrastructure.Repositories;
using DogsWebApi.Presentation.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
// Configure and register the database context with a connection string.
builder.Services.AddDbContext<DogContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DogDatabase")));

// Register dependencies for Onion Architecture layers.
// Infrastructure Layer
builder.Services.AddScoped<IDogRepository, DogRepository>();

builder.Services.AddScoped<IDogService, DogService>();

builder.Services.AddSingleton<IRequestObserver>(new RequestRateLimiter(10)); // Limit to 10 requests per second

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<RateLimitingMiddleware>();

app.MapControllers();

app.Run();