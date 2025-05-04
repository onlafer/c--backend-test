public interface IDiskCache
{
    Task<CacheResult<T>> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);
    Task RemoveAsync(string key);
    Task<bool> ExistsAsync(string key);
}

public record CacheResult<T>(T Value, bool FromCache);

public class DiskCache : IDiskCache
{
    private readonly string _cacheDirectory;
    private readonly ILogger<DiskCache> _logger;

    public DiskCache(ILogger<DiskCache> logger)
    {
        _logger = logger;
        _cacheDirectory = Path.Combine(Directory.GetCurrentDirectory(), "DiskCache");
        
        if (!Directory.Exists(_cacheDirectory))
        {
            Directory.CreateDirectory(_cacheDirectory);
        }
    }

    public async Task<CacheResult<T>> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
    {
        var filePath = GetFilePath(key);
        
        if (File.Exists(filePath))
        {
            var cacheItem = await ReadFromDiskAsync<CacheItem<T>>(filePath);
            if (cacheItem.Expiration > DateTime.UtcNow)
            {
                _logger.LogInformation($"Disk Cache: получено значение по ключу {key}");
                return new CacheResult<T>(cacheItem.Value, true);
            }
            
            File.Delete(filePath);
        }

        var value = await factory();
        var expirationTime = DateTime.UtcNow.Add(expiration ?? TimeSpan.FromMinutes(5));
        
        await SaveToDiskAsync(filePath, new CacheItem<T>(value, expirationTime));
        _logger.LogInformation($"Disk Cache: сохранено значение по ключу {key}");

        return new CacheResult<T>(value, false);
    }

    public Task RemoveAsync(string key)
    {
        var filePath = GetFilePath(key);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            _logger.LogInformation($"Disk Cache: удалено значение по ключу {key}");
        }
        
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(string key)
    {
        var filePath = GetFilePath(key);
        if (!File.Exists(filePath)) return false;
        
        var cacheItem = await ReadFromDiskAsync<CacheItem<object>>(filePath);
        return cacheItem.Expiration > DateTime.UtcNow;
    }

    private string GetFilePath(string key)
    {
        var safeKey = Path.GetInvalidFileNameChars()
            .Aggregate(key, (current, c) => current.Replace(c, '_'));
        return Path.Combine(_cacheDirectory, $"{safeKey}.cache");
    }

    private async Task<T> ReadFromDiskAsync<T>(string filePath)
    {
        var json = await File.ReadAllTextAsync(filePath);
        return System.Text.Json.JsonSerializer.Deserialize<T>(json);
    }

    private async Task SaveToDiskAsync<T>(string filePath, T item)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(item);
        await File.WriteAllTextAsync(filePath, json);
    }

    private record CacheItem<T>(T Value, DateTime Expiration);
}
