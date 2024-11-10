using DogsWebApi.Domain.Entities;
using DogsWebApi.Infrastructure.Interfaces;
using DogsWebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DogsWebApi.Infrastructure.Repositories;

public class DogRepository : IDogRepository
{
    private readonly DogContext _context;

    public DogRepository(DogContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Dog>> GetDogsAsync(string attribute, string order)
    {
        var dogs = _context.Dogs.AsQueryable();
        return attribute switch
        {
            "weight" => order == "desc"
                ? await dogs.OrderByDescending(d => d.Weight).ToListAsync()
                : await dogs.OrderBy(d => d.Weight).ToListAsync(),
            "tail_length" => order == "desc"
                ? await dogs.OrderByDescending(d => d.TailLength).ToListAsync()
                : await dogs.OrderBy(d => d.TailLength).ToListAsync(),
            _ => await dogs.ToListAsync()
        };
    }

    public Task<Dog> GetDogByNameAsync(string name)
    {
        return _context.Dogs.FirstOrDefaultAsync(d => d.Name == name);
    }

    public async Task CreateDogAsync(Dog dog)
    {
        _context.Dogs.Add(dog);
        await _context.SaveChangesAsync();
    }
}