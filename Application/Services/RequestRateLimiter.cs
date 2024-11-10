using DogsWebApi.Application.Interfaces;

namespace DogsWebApi.Application.Services;

public class RequestRateLimiter : IRequestObserver
{
    private readonly int _requestLimit;
    private int _requestCount;
    private DateTime _resetTime;
    private readonly SemaphoreSlim _semaphore;

    public RequestRateLimiter(int requestLimit)
    {
        _requestLimit = requestLimit;
        _requestCount = 0;
        _resetTime = DateTime.UtcNow.AddSeconds(1);
        _semaphore = new SemaphoreSlim(_requestLimit, _requestLimit);
    }

    public bool CanProcessRequest()
    {
        lock (this)
        {
            if (DateTime.UtcNow >= _resetTime)
            {
                _requestCount = 0;
                _resetTime = DateTime.UtcNow.AddSeconds(1);
            }

            if (_requestCount >= _requestLimit) return false;

            _requestCount++;
            return true;
        }
    }

    public void UpdateRequestCount()
    {
        _semaphore.Wait();
        try
        {
            _requestCount++;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}