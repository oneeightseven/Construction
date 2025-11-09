namespace Construction.Service.Interfaces
{
    public interface IMinioCacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T data, TimeSpan maxLifetime);
        Task<bool> ExistsAsync(string key);
        Task RemoveAsync(string key);
    }
}
