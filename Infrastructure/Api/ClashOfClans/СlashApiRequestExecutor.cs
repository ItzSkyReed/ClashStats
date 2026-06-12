using System.Net.Http.Json;
using System.Text.Json;
using Application.ClashOfClansModels.Common;
using Microsoft.Extensions.Options;

namespace Infrastructure.Api.ClashOfClans;

public class ClashApiRequestExecutor(HttpClient client, IOptions<JsonSerializerOptions> options)
{
    private readonly JsonSerializerOptions _jsonOptions = options.Value;

    public async Task<ApiResult<T>> GetAsync<T>(string endpoint, CancellationToken ct)
    {
        var response = await client.GetAsync(endpoint, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (response.IsSuccessStatusCode)
        {
            var data = JsonSerializer.Deserialize<T>(body, _jsonOptions);
            return new ApiResult<T>(data, null, response.StatusCode);
        }

        var error = JsonSerializer.Deserialize<ClientErrorDto>(body);
        return new ApiResult<T>(default, error, response.StatusCode);
    }

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