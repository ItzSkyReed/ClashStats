using Application.ClashOfClansModels.Clans;
using Application.ClashOfClansModels.Clans.Capital;
using Application.ClashOfClansModels.Clans.ClanWarLeagues;
using Application.ClashOfClansModels.Clans.ClanWars;
using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.WebUtilities;

namespace Infrastructure.Api.ClashOfClans.ApiClient;

public partial class ClashApiClient
{
    public async Task<IApiResult<ClanDto>> GetClanAsync(string clanTag, CancellationToken cancellationToken = default)
    {
        var encoded = Uri.EscapeDataString(clanTag);
        return await _executor.GetAsync<ClanDto>($"clans/{encoded}", cancellationToken);
    }

    public async Task<IApiResult<ClanMemberListDto>> GetClanMembersAsync(
        string clanTag, uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var encodedTag = Uri.EscapeDataString(clanTag);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString($"clans/{encodedTag}/members", queryParams);

        return await _executor.GetAsync<ClanMemberListDto>(endpoint, cancellationToken);
    }

    public async Task<IApiResult<ClanCapitalRaidSeasonsDto>> GetClanCapitalRaidSeasonsAsync(
        string clanTag, uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var encodedTag = Uri.EscapeDataString(clanTag);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString($"clans/{encodedTag}/capitalraidseasons", queryParams);

        return await _executor.GetAsync<ClanCapitalRaidSeasonsDto>(endpoint, cancellationToken);
    }

    public async Task<IApiResult<ClanWarLogDto>> GetClanWarLogAsync(
        string clanTag, uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var encodedTag = Uri.EscapeDataString(clanTag);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString($"clans/{encodedTag}/warlog", queryParams);

        return await _executor.GetAsync<ClanWarLogDto>(endpoint, cancellationToken);
    }

    public async Task<IApiResult<ClanWarDto>> GetCurrentClanWarAsync(string clanTag, CancellationToken cancellationToken = default)
    {
        var encoded = Uri.EscapeDataString(clanTag);
        return await _executor.GetAsync<ClanWarDto>($"clans/{encoded}/currentwar", cancellationToken);
    }

    public async Task<IApiResult<ClanListDto>> GetClansAsync(
        string? name = null, WarFrequency? warFrequency = null, uint? limit = null, uint? locationId = null,
        string? after = null, string? before = null, uint? maxMembers = null, uint? minMembers = null,
        uint? minClanPoints = null, uint? minClanLevel = null, List<uint>? labelIds = null, CancellationToken cancellationToken = default)
    {
        if (name is not null && name.Length < 3)
            throw new ArgumentException("Name must be at least 3 characters long if present");

        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "name", name);
        AddQueryParam(queryParams, "warFrequency", warFrequency);
        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "locationId", locationId);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);
        AddQueryParam(queryParams, "maxMembers", maxMembers);
        AddQueryParam(queryParams, "minMembers", minMembers);
        AddQueryParam(queryParams, "minClanPoints", minClanPoints);
        AddQueryParam(queryParams, "minClanLevel", minClanLevel);
        AddQueryParam(queryParams, "labelIds", labelIds);

        var endpoint = QueryHelpers.AddQueryString("clans", queryParams);

        return await _executor.GetAsync<ClanListDto>(endpoint, cancellationToken);
    }

    public async Task<IApiResult<ClanWarLeagueGroupDto>> GetCurrentClanWarLeagueGroupAsync(string clanTag,
        CancellationToken cancellationToken = default)
    {
        var encodedTag = Uri.EscapeDataString(clanTag);
        return await _executor.GetAsync<ClanWarLeagueGroupDto>($"clans/{encodedTag}/currentwar/leaguegroup", cancellationToken);
    }

    public async Task<IApiResult<ClanWarLeaguerWarDto>> GetClanWarLeagueWarAsync(string warTag, CancellationToken cancellationToken = default)
    {
        var encodedTag = Uri.EscapeDataString(warTag);
        return await _executor.GetAsync<ClanWarLeaguerWarDto>($"clanwarleagues/wars/{encodedTag}", cancellationToken);
    }
}