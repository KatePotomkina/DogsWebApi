
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace DogsWebApiTest;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

public class PingControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PingControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Ping_ReturnsVersion()
    {
        var response = await _client.GetAsync("/ping");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Equal("Dogshouseservice.Version1.0.1", responseString);
    }
}
