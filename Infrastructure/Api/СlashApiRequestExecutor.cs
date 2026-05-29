using System.Net.Http.Json;
using System.Text.Json;
using Application.DTOs.Common;
using Microsoft.Extensions.Options;

namespace Infrastructure.Api;

public class ClashApiRequestExecutor(HttpClient client, IOptions<JsonSerializerOptions> options)
{
    private readonly JsonSerializerOptions _jsonOptions = options.Value;

    public async Task<ApiResult<T>> GetAsync<T>(string endpoint)
    {
        var response = await client.GetAsync(endpoint);
        var body = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var data = JsonSerializer.Deserialize<T>(body, _jsonOptions);
            return new ApiResult<T>(data, null, response.StatusCode);
        }

        var error = JsonSerializer.Deserialize<ClientErrorDto>(body);
        return new ApiResult<T>(default, error, response.StatusCode);
    }

    public async Task<ApiResult<T>> PostAsync<T>(string endpoint, Dictionary<string, object> body)
    {
        var response = await client.PostAsJsonAsync(endpoint, body);

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<T>();
            return new ApiResult<T>(data, null, response.StatusCode);
        }

        var errorBody = await response.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<ClientErrorDto>(errorBody);

        return new ApiResult<T>(default, error, response.StatusCode);
    }
}