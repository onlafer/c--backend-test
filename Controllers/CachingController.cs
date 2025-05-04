using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCaching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

[ApiController]
[Route("[controller]")]
public class CachingController : ControllerBase
{
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _distributedCache;
    private readonly IDiskCache _diskCache;
    private readonly ILogger<CachingController> _logger;

    public CachingController(
        IMemoryCache memoryCache,
        IDistributedCache distributedCache,
        IDiskCache diskCache,
        ILogger<CachingController> logger)
    {
        _memoryCache = memoryCache;
        _distributedCache = distributedCache;
        _diskCache = diskCache;
        _logger = logger;
    }

    [HttpGet("memory-cache")]
    public async Task<ActionResult<string>> GetWithMemoryCache()
    {
        const string cacheKey = "memory_cache_demo";


        if (_memoryCache.TryGetValue(cacheKey, out string cachedValue))
        {
            _logger.LogInformation("Получено из In-Memory кэша");
            return Ok(cachedValue);
        }


        var value = await GetExpensiveValueAsync();


        _memoryCache.Set(cacheKey, value, TimeSpan.FromSeconds(30));

        _logger.LogInformation("Сохранено в In-Memory кэш");
        return Ok(value);
    }

    [HttpGet("distributed-cache")]
    public async Task<ActionResult<string>> GetWithDistributedCache()
    {
        const string cacheKey = "distributed_cache_demo";


        var cachedValue = await _distributedCache.GetStringAsync(cacheKey);
        if (cachedValue != null)
        {
            _logger.LogInformation("Получено из распределенного кэша");
            return Ok(cachedValue);
        }


        var value = await GetExpensiveValueAsync();


        await _distributedCache.SetStringAsync(
            cacheKey,
            value,
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) });

        _logger.LogInformation("Сохранено в распределенный кэш");
        return Ok(value);
    }

    [HttpGet("response-cache")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public async Task<ActionResult<string>> GetWithResponseCache()
    {
        _logger.LogInformation("Выполняется метод с Response Caching");
        return Ok(await GetExpensiveValueAsync());
    }

    [HttpGet("disk-cache")]
    public async Task<ActionResult<string>> GetWithDiskCache()
    {
        const string cacheKey = "disk_cache_demo";

        var value = await _diskCache.GetOrCreateAsync(
            cacheKey,
            async () => await GetExpensiveValueAsync(),
            TimeSpan.FromMinutes(5));

        _logger.LogInformation(value.FromCache
            ? "Получено из дискового кэша"
            : "Сохранено в дисковый кэш");

        return Ok(value.Value);
    }

    [HttpPost("clear-caches")]
    public async Task<IActionResult> ClearCaches()
    {
        const string memoryCacheKey = "memory_cache_demo";
        const string distributedCacheKey = "distributed_cache_demo";
        const string diskCacheKey = "disk_cache_demo";

        _memoryCache.Remove(memoryCacheKey);
        await _distributedCache.RemoveAsync(distributedCacheKey);
        await _diskCache.RemoveAsync(diskCacheKey);

        _logger.LogInformation("Все кэши очищены");
        return Ok("Все кэши успешно очищены");
    }

    private async Task<string> GetExpensiveValueAsync()
    {
        await Task.Delay(1000);
        return $"Значение сгенерировано в {DateTime.Now:HH:mm:ss.fff}";
    }
}