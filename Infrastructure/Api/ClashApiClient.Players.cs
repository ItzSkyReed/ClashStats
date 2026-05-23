using Application.DTOs.Common;
using Application.DTOs.Players;
using Application.Interfaces;

namespace Infrastructure.Api;

public partial class ClashApiClient
{
    public async Task<IApiResult<PlayerDto>> GetPlayerAsync(string playerTag)
    {
        var encodedTag = Uri.EscapeDataString(playerTag);
        return await _executor.GetAsync<PlayerDto>($"players/{encodedTag}");
    }

    public async Task<IApiResult<VerifyTokenResponseDto>> PostVerifyTokenAsync(string playerTag, string token)
    {
        var encodedTag = Uri.EscapeDataString(playerTag);
        var bodyParams = new Dictionary<string, object>
        {
            { "token", token }
        };
        return await _executor.PostAsync<VerifyTokenResponseDto>($"players/{encodedTag}/verify_token", bodyParams);
    }
}