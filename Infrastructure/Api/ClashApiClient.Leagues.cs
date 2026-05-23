using System.Text.RegularExpressions;
using Application.DTOs.Leagues;
using Application.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace Infrastructure.Api;

public partial class ClashApiClient
{
    [GeneratedRegex(@"^\d{4}-\d{2}(?:-\d{2})?$", RegexOptions.Compiled)]
    private static partial Regex SeasonIdRegexPattern();

    public async Task<IApiResult<LeagueListDto>> GetLeaguesAsync(uint? limit = null, string? after = null, string? before = null)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString("leagues", queryParams);

        return await _executor.GetAsync<LeagueListDto>(endpoint);
    }

    public async Task<IApiResult<LeagueDto>> GetLeagueAsync(uint leagueId)
    {
        return await _executor.GetAsync<LeagueDto>($"leagues/{leagueId}");
    }

    public async Task<IApiResult<BuilderBaseLeagueListDto>> GetBuilderBaseLeaguesAsync(uint? limit = null, string? after = null, string? before = null)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString("builderbaseleagues", queryParams);

        return await _executor.GetAsync<BuilderBaseLeagueListDto>(endpoint);
    }

    public async Task<IApiResult<BuilderBaseLeagueDto>> GetBuilderBaseLeagueAsync(uint leagueId)
    {
        return await _executor.GetAsync<BuilderBaseLeagueDto>($"builderbaseleagues/{leagueId}");
    }


    public async Task<IApiResult<CapitalLeagueListDto>> GetCapitalLeaguesAsync(uint? limit = null, string? after = null, string? before = null)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString("capitalleagues", queryParams);

        return await _executor.GetAsync<CapitalLeagueListDto>(endpoint);
    }

    public async Task<IApiResult<CapitalLeagueDto>> GetCapitalLeagueAsync(uint leagueId)
    {
        return await _executor.GetAsync<CapitalLeagueDto>($"capitalleagues/{leagueId}");
    }

    public async Task<IApiResult<LeagueTierListDto>> GetLeagueTiersAsync(uint? limit = null, string? after = null, string? before = null)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString("leaguetiers", queryParams);

        return await _executor.GetAsync<LeagueTierListDto>(endpoint);
    }

    public async Task<IApiResult<LeagueListDto>> GetLeagueTierAsync(uint leagueTierId)
    {
        return await _executor.GetAsync<LeagueListDto>($"leaguetiers/{leagueTierId}");
    }

    public async Task<IApiResult<WarLeagueListDto>> GetWarLeaguesAsync(uint? limit = null, string? after = null, string? before = null)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString("warleagues", queryParams);

        return await _executor.GetAsync<WarLeagueListDto>(endpoint);
    }

    public async Task<IApiResult<WarLeagueDto>> GetWarLeagueAsync(uint leagueId)
    {
        return await _executor.GetAsync<WarLeagueDto>($"warleagues/{leagueId}");
    }

    /// <param name="leagueId">Now this works only for leagueId = 29000022 (Legend League) </param>
    public async Task<IApiResult<LeagueSeasonListDto>> GetLeagueSeasonsAsync(uint leagueId = 29000022)
    {
        return await _executor.GetAsync<LeagueSeasonListDto>($"leagues/{leagueId}/seasons");
    }

    public async Task<IApiResult<WarLeagueListDto>> GetLeagueSeasonRankingsAsync(string seasonId, uint leagueId = 29000022, uint? limit = null, string? after = null, string? before = null)
    {

        ValidateExclusiveBeforeAfter(before, after);

        if (limit is < 100 or > 25000)
            throw new ArgumentException("Pagination parameter 'limit' has to be between 100 and 25 000");

        if (!SeasonIdRegexPattern().IsMatch(seasonId))
            throw new ArgumentException("Invalid season id format, should be YYYY-MM or YYYY-MM-DD");

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString($"leagues/{leagueId}/seasons/{seasonId}", queryParams);

        return await _executor.GetAsync<WarLeagueListDto>(endpoint);
    }
}