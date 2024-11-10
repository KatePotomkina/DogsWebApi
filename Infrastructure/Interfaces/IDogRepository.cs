using DogsWebApi.Domain.Entities;

namespace DogsWebApi.Infrastructure.Interfaces;

public interface IDogRepository
{
    Task<IEnumerable<Dog>> GetDogsAsync(string attribute, string order);
    Task<Dog> GetDogByNameAsync(string name);
    Task CreateDogAsync(Dog dog);
}