using Microsoft.Extensions.Caching.Memory;

namespace DogsWebApi.Presentation.Middleware;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly int _requestLimit;
    private readonly TimeSpan _timeSpan;
    private readonly IMemoryCache _cache;

    public RateLimitingMiddleware(RequestDelegate next, IConfiguration configuration, IMemoryCache cache)
    {
        _next = next;
        _cache = cache;

        _requestLimit = configuration.GetValue<int>("RateLimiting:RequestsPerSecond", 10);
        _timeSpan = TimeSpan.FromSeconds(1);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clientIp = context.Connection.RemoteIpAddress?.ToString();
        var cacheKey = $"RateLimit_{clientIp}";

        var requestCount = _cache.GetOrCreate(cacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _timeSpan;
            return 0;
        });

        if (requestCount >= _requestLimit)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.Response.WriteAsync("Too Many Requests. Please try again later.");
            return;
        }

        _cache.Set(cacheKey, requestCount + 1, _timeSpan);
        await _next(context);
    }
}