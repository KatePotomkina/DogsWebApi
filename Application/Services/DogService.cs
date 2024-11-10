using DogsWebApi.Application.Interfaces;
using DogsWebApi.Domain.Entities;
using DogsWebApi.Domain.Validation;
using DogsWebApi.Infrastructure.Interfaces;

namespace DogsWebApi.Application.Services;

public class DogService : IDogService
{
    private readonly IDogRepository _dogRepository;
    private readonly IRequestObserver _requestObserver;

    public DogService(IDogRepository dogRepository, IRequestObserver requestObserver)
    {
        _dogRepository = dogRepository;
        _requestObserver = requestObserver;
    }

    public async Task<IEnumerable<Dog>> GetDogsAsync(string attribute, string order, int pageNumber, int pageSize)
    {
        if (!_requestObserver.CanProcessRequest())
            throw new Exception("Too Many Requests");

        _requestObserver.UpdateRequestCount();

        var dogs = await _dogRepository.GetDogsAsync(attribute, order);
        return dogs.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }

    public async Task<Dog> CreateDogAsync(Dog dog)
    {
        if (!_requestObserver.CanProcessRequest())
            throw new Exception("Too Many Requests");

        _requestObserver.UpdateRequestCount();

        var existingDog = await _dogRepository.GetDogByNameAsync(dog.Name);
        if (existingDog != null)
            throw new Exception("A dog with this name already exists.");

        var newDog = new DogBuilder()
            .SetName(dog.Name)
            .SetColor(dog.Color)
            .SetTailLength(dog.TailLength)
            .SetWeight(dog.Weight)
            .Build();

        await _dogRepository.CreateDogAsync(newDog);
        return newDog;
    }

    public Task<string> PingAsync()
    {
        return Task.FromResult("Dogshouseservice.Version1.0.1");
    }
}