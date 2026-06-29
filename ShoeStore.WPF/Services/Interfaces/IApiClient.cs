namespace ShoeStore.WPF.Services.Interfaces;

public interface IApiClient
{
    Task<T?> GetAsync<T>(string endpoint);
    Task<T?> PostAsync<T>(string endpoint, object data);
    Task<T?> PutAsync<T>(string endpoint, object data);
    Task<bool> DeleteAsync(string endpoint);
    void SetAuthToken(string token);
    void ClearAuthToken();
}