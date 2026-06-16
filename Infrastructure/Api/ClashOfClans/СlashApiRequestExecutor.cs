using System.Net.Http.Json;
using System.Text.Json;
using Application.ClashOfClansModels.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Api.ClashOfClans;

public partial class ClashApiRequestExecutor(HttpClient client, IOptions<JsonSerializerOptions> options, ILogger<ClashApiRequestExecutor> logger)
{
    private readonly JsonSerializerOptions _jsonOptions = options.Value;

    public async Task<ApiResult<T>> GetAsync<T>(string endpoint, CancellationToken ct)
    {
        var response = await client.GetAsync(endpoint, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (response.IsSuccessStatusCode)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                LogEmptyBody(endpoint);
                return new ApiResult<T>(default,
                    new ClientErrorDto { Reason = "API returned an empty successful response body." },
                    response.StatusCode);
            }

            var data = JsonSerializer.Deserialize<T>(body, _jsonOptions);
            return new ApiResult<T>(data, null, response.StatusCode);
        }

        if (string.IsNullOrWhiteSpace(body))
        {
            LogEmptyErrorBody(endpoint, (int)response.StatusCode);

            return new ApiResult<T>(default,
                new ClientErrorDto
                {
                    Reason = $"API returned error HTTP {(int)response.StatusCode} with an empty body."
                },
                response.StatusCode);
        }

        var error = JsonSerializer.Deserialize<ClientErrorDto>(body, _jsonOptions);
        return new ApiResult<T>(default, error, response.StatusCode);
    }


    [LoggerMessage(LogLevel.Error, "Api returned empty body for SUCCESS response, endpoint: {endpoint}")]
    partial void LogEmptyBody(string endpoint);

    [LoggerMessage(LogLevel.Warning, "Api returned error status {statusCode} with empty body, endpoint: {endpoint}")]
    partial void LogEmptyErrorBody(string endpoint, int statusCode);

    public async Task<ApiResult<T>> PostAsync<T>(string endpoint, Dictionary<string, object> body, CancellationToken ct)
    {
        var response = await client.PostAsJsonAsync(endpoint, body, ct);

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<T>(ct);
            return new ApiResult<T>(data, null, response.StatusCode);
        }

        var errorBody = await response.Content.ReadAsStringAsync(ct);
        var error = JsonSerializer.Deserialize<ClientErrorDto>(errorBody);

        return new ApiResult<T>(default, error, response.StatusCode);
    }
}