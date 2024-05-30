namespace DemoLibrary.Infraestructure.Messaging._Sync;

public interface IHttpPublisher
{
    Task<string> GetAsync(string url, Dictionary<string, string>? headers);
    Task<string> PostAsync(string url, object data);
    Task<string> PutAsync(string url, object data);
    Task<string> DeleteAsync(string url);
}