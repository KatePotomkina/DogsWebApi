using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

public class RateLimitsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public RateLimitsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task TooManyRequests_Returns429()
    {
        // Перший запит
        var response1 = await _client.GetAsync("/dogs");
        response1.EnsureSuccessStatusCode();

        // Другий запит, що перевищує ліміт
        var response2 = await _client.GetAsync("/dogs");
        Assert.Equal(429, (int)response2.StatusCode); // 429 Too Many Requests
    }
}