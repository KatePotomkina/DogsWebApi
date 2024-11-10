using DogsWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DogsWebApi.Infrastructure.Persistence;

public class DogContext : DbContext
{
    public DbSet<Dog> Dogs { get; set; }

    public DogContext(DbContextOptions<DogContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dog>().HasIndex(d => d.Name).IsUnique();
    }
}