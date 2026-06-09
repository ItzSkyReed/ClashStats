using System.Net;
using System.Runtime.CompilerServices;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Extensions;

public static partial class ApiResultExtensions
{
    public static async Task<T?> UnwrapOrLogAsync<T>(
        this Task<IApiResult<T>> task,
        ILogger logger,
        [CallerMemberName] string method = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0)
    {
        var result = await task;

        if (result.IsSuccess)
            return result.Data;

        var errorMessage = result.Error?.Message ?? "No specific error message provided by the API.";

        var fileName = Path.GetFileName(file);

        logger.LogApiError(fileName, method, line, result.StatusCode, (int)result.StatusCode, errorMessage);

        return default;
    }

    [LoggerMessage(LogLevel.Error,
        "Api error {FileName} -> {Method} (line {Line}). HTTP Status: {StatusCode} ({StatusCodeInt}). Message: {ErrorMessage}")]
    static partial void LogApiError(this ILogger logger, string fileName, string method, int line, HttpStatusCode statusCode, int statusCodeInt,
        string errorMessage);
}