using DogsWebApi.Domain.Entities;

namespace DogsWebApi.Application.Interfaces;

public interface IDogService
{
    Task<IEnumerable<Dog>> GetDogsAsync(string attribute, string order, int pageNumber, int pageSize);
    Task<Dog> CreateDogAsync(Dog dog);
    Task<string> PingAsync();
}