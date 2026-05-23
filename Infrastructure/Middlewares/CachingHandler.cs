using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Middlewares;

using System.Net;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

public partial class CachingHandler(IMemoryCache memoryCache, ILogger<CachingHandler> logger) : DelegatingHandler
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> Locks = new();

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Method != HttpMethod.Get || request.RequestUri == null)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var cacheKey = $"clash_api_{request.RequestUri}";

        if (memoryCache.TryGetValue(cacheKey, out string? cachedResponse))
        {
            return CreateCachedResponse(cachedResponse!);
        }

        var semaphore = Locks.GetOrAdd(cacheKey, _ => new SemaphoreSlim(1, 1));

        await semaphore.WaitAsync(cancellationToken);

        try
        {
            // Double-check after waiting
            if (memoryCache.TryGetValue(cacheKey, out cachedResponse))
            {
                return CreateCachedResponse(cachedResponse!);
            }

            LogCacheMissForUrlFetchingFromApi(request.RequestUri);

            var response = await base.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return response;
            }

            var contentString = await response.Content.ReadAsStringAsync(cancellationToken);

            memoryCache.Set(
                cacheKey,
                contentString,
                TimeSpan.FromSeconds(30));

            return CreateCachedResponse(contentString);
        }
        finally
        {
            semaphore.Release();
        }
    }

    private static HttpResponseMessage CreateCachedResponse(string content)
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(content, Encoding.UTF8, "application/json")
        };
    }

    [LoggerMessage(LogLevel.Debug, "Cache HIT for {Url}")]
    partial void LogCacheHitForUrl(Uri url);

    [LoggerMessage(LogLevel.Debug, "Cache MISS for {Url}. Fetching from API...")]
    partial void LogCacheMissForUrlFetchingFromApi(Uri url);
}