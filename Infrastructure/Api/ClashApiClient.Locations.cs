using Application.DTOs.Clans.BuilderBase;
using Application.DTOs.Clans.Capital;
using Application.DTOs.Locations;
using Application.DTOs.Players;
using Application.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace Infrastructure.Api;

public partial class ClashApiClient
{
    public async Task<IApiResult<LocationListDto>> GetLocationAsync(int locationId)
    {
        return await _executor.GetAsync<LocationListDto>($"locations/{locationId}");
    }

    public async Task<IApiResult<LocationListDto>> GetLocationsAsync(uint? limit = null, string? after = null, string? before = null)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);
        var endpoint = QueryHelpers.AddQueryString("locations", queryParams);

        return await _executor.GetAsync<LocationListDto>(endpoint);
    }

    public async Task<IApiResult<ClanCapitalRankingListDto>> GetCapitalRankingsByLocationAsync(uint locationId, uint? limit = null, string? after = null, string? before = null)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString($"locations/{locationId}/rankings/capitals", queryParams);

        return await _executor.GetAsync<ClanCapitalRankingListDto>(endpoint);
    }

    public async Task<IApiResult<ClanBuilderBaseRankingListDto>> GetClanBuilderBaseRankingsByLocationAsync(uint locationId, uint? limit = null, string? after = null, string? before = null)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString($"locations/{locationId}/rankings/clans-builder-base", queryParams);

        return await _executor.GetAsync<ClanBuilderBaseRankingListDto>(endpoint);
    }

    public async Task<IApiResult<PlayerBuilderBaseRankingListDto>> GetPlayerBuilderBaseRankingsByLocationAsync(uint locationId, uint? limit = null, string? after = null, string? before = null)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString($"locations/{locationId}/rankings/players-builder-base", queryParams);

        return await _executor.GetAsync<PlayerBuilderBaseRankingListDto>(endpoint);
    }

    public async Task<IApiResult<PlayerBuilderBaseRankingListDto>> GetPlayerRankingsByLocationAsync(uint locationId, uint? limit = null, string? after = null, string? before = null)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString($"locations/{locationId}/rankings/players", queryParams);

        return await _executor.GetAsync<PlayerBuilderBaseRankingListDto>(endpoint);
    }

}