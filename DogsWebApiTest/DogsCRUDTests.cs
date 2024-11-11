using System.Text;
using DogsWebApi.Domain.Entities;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace DogsWebApiTest;

public class DogsCRUDTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public DogsCRUDTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AddDog_ReturnsCreatedDog()
    {
        var newDog = new Dog
        {
            Name = "Doggy",
            Color = "red",
            TailLength = 173,
            Weight = 33
        };

        var content = new StringContent(JsonConvert.SerializeObject(newDog), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/dog", content);
        response.EnsureSuccessStatusCode();

        var createdDog = JsonConvert.DeserializeObject<Dog>(await response.Content.ReadAsStringAsync());
        Assert.Equal(newDog.Name, createdDog.Name);
        Assert.Equal(newDog.Color, createdDog.Color);
        Assert.Equal(newDog.TailLength, createdDog.TailLength);
        Assert.Equal(newDog.Weight, createdDog.Weight);
    }

    [Fact]
    public async Task AddDog_DuplicateName_ReturnsBadRequest()
    {
        var newDog = new Dog
        {
            Name = "Neo",
            Color = "blue",
            TailLength = 50,
            Weight = 20
        };

        var content = new StringContent(JsonConvert.SerializeObject(newDog), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/dog", content);

        Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task AddDog_InvalidTailLength_ReturnsBadRequest()
    {
        var newDog = new Dog
        {
            Name = "Max",
            Color = "black",
            TailLength = -5,
            Weight = 30
        };

        var content = new StringContent(JsonConvert.SerializeObject(newDog), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/dog", content);

        Assert.Equal(400, (int)response.StatusCode);
    }
}