namespace DogsWebApi.Application.Interfaces;

public interface IRequestObserver
{
    bool CanProcessRequest();
    void UpdateRequestCount();
}