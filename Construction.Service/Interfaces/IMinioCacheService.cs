namespace Construction.Service.Interfaces
{
    public interface IMinioCacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T data, int days = 90);
        Task<bool> ExistsAsync(string key);
        Task RemoveAsync(string key);
    }
}
