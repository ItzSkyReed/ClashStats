using System.Collections;
using Application.Interfaces;

namespace Infrastructure.Api.ClashOfClans.ApiClient;

/// <summary>
/// Клиент для взаимодействия с API Clash of Clans.
/// </summary>
public partial class ClashApiClient : IClashApiClient
{
    private readonly ClashApiRequestExecutor _executor;


    /// <param name="executor">Executor from DI</param>
    public ClashApiClient(ClashApiRequestExecutor executor)
    {
        _executor = executor;
    }

    private static void ValidateExclusiveBeforeAfter(in string? before = null, in string? after = null)
    {
        if (!string.IsNullOrEmpty(after) && !string.IsNullOrEmpty(before))
            throw new ArgumentException("Specify only one: after or before.");
    }

    private static void AddQueryParam(Dictionary<string, string?> queryParams, string key, object? value)
    {
        switch (value)
        {
            case null:
                return;
            case string str:
            {
                if (!string.IsNullOrWhiteSpace(str))
                    queryParams[key] = str;
                return;
            }
            case IEnumerable enumerable:
            {
                var items = enumerable.Cast<object>()
                    .Select(x => x.ToString())
                    .Where(x => !string.IsNullOrWhiteSpace(x));

                var joined = string.Join(",", items);

                if (!string.IsNullOrEmpty(joined))
                    queryParams[key] = joined;

                return;
            }
            default:
                queryParams[key] = value.ToString()!;
                break;
        }
    }
}