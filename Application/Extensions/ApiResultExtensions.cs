using System.Runtime.CompilerServices;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Extensions;

public static class ApiResultExtensions
{
    public static async Task<T?> UnwrapOrLogAsync<T>(
        this Task<IApiResult<T>> task,
        ILogger logger,
        [CallerMemberName] string method = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0
    )

    {
        var result = await task;

        if (result.IsSuccess)
            return result.Data;

        logger.LogError("Api error {file} -> {method} (line {line}): {ErrorMessage}", file, method, line, result.Error?.Message);

        return default;
    }
}