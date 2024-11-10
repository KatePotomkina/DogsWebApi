using DogsWebApi.Domain.Entities;

namespace DogsWebApi.Domain.Validation;

public class DogBuilder
{
    private readonly Dog _dog = new();

    public DogBuilder SetName(string name)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be empty.");
        _dog.Name = name;
        return this;
    }

    public DogBuilder SetColor(string color)
    {
        _dog.Color = color;
        return this;
    }

    public DogBuilder SetTailLength(int tailLength)
    {
        if (tailLength < 0) throw new ArgumentException("Tail length must be non-negative.");
        _dog.TailLength = tailLength;
        return this;
    }

    public DogBuilder SetWeight(int weight)
    {
        if (weight < 0) throw new ArgumentException("Weight must be non-negative.");
        _dog.Weight = weight;
        return this;
    }

    public Dog Build()
    {
        return _dog;
    }
}