using Application.ClashOfClansModels.Clans.BuilderBase;
using Application.ClashOfClansModels.Clans.Capital;
using Application.ClashOfClansModels.Locations;
using Application.ClashOfClansModels.Players;
using Application.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace Infrastructure.Api;

public partial class ClashApiClient
{
    public async Task<IApiResult<LocationListDto>> GetLocationAsync(int locationId, CancellationToken cancellationToken = default)
    {
        return await _executor.GetAsync<LocationListDto>($"locations/{locationId}", cancellationToken);
    }

    public async Task<IApiResult<LocationListDto>> GetLocationsAsync(
        uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);
        var endpoint = QueryHelpers.AddQueryString("locations", queryParams);

        return await _executor.GetAsync<LocationListDto>(endpoint, cancellationToken);
    }

    public async Task<IApiResult<ClanCapitalRankingListDto>> GetCapitalRankingsByLocationAsync(
        uint locationId, uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString($"locations/{locationId}/rankings/capitals", queryParams);

        return await _executor.GetAsync<ClanCapitalRankingListDto>(endpoint, cancellationToken);
    }

    public async Task<IApiResult<ClanBuilderBaseRankingListDto>> GetClanBuilderBaseRankingsByLocationAsync(
        uint locationId, uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString($"locations/{locationId}/rankings/clans-builder-base", queryParams);

        return await _executor.GetAsync<ClanBuilderBaseRankingListDto>(endpoint, cancellationToken);
    }

    public async Task<IApiResult<PlayerBuilderBaseRankingListDto>> GetPlayerBuilderBaseRankingsByLocationAsync(
        uint locationId, uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString($"locations/{locationId}/rankings/players-builder-base", queryParams);

        return await _executor.GetAsync<PlayerBuilderBaseRankingListDto>(endpoint, cancellationToken);
    }

    public async Task<IApiResult<PlayerBuilderBaseRankingListDto>> GetPlayerRankingsByLocationAsync(
        uint locationId, uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString($"locations/{locationId}/rankings/players", queryParams);

        return await _executor.GetAsync<PlayerBuilderBaseRankingListDto>(endpoint, cancellationToken);
    }
}