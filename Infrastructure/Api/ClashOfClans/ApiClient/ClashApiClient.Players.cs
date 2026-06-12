using Application.ClashOfClansModels.Common;
using Application.ClashOfClansModels.Players;
using Application.Interfaces;

namespace Infrastructure.Api.ClashOfClans.ApiClient;

public partial class ClashApiClient
{
    public async Task<IApiResult<PlayerDto>> GetPlayerAsync(string playerTag, CancellationToken ct = default)
    {
        var encodedTag = Uri.EscapeDataString(playerTag);
        return await _executor.GetAsync<PlayerDto>($"players/{encodedTag}", ct);
    }

    public async Task<IApiResult<VerifyTokenResponseDto>> PostVerifyTokenAsync(string playerTag, string token, CancellationToken ct = default)
    {
        var encodedTag = Uri.EscapeDataString(playerTag);
        var bodyParams = new Dictionary<string, object>
        {
            { "token", token }
        };
        return await _executor.PostAsync<VerifyTokenResponseDto>($"players/{encodedTag}/verify_token", bodyParams, ct);
    }
}