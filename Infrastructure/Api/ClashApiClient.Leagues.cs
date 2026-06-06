using System.Text.RegularExpressions;
using Application.DTOs.Leagues;
using Application.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace Infrastructure.Api;

public partial class ClashApiClient
{
    public async Task<IApiResult<LeagueListDto>> GetLeaguesAsync(
        uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString("leagues", queryParams);

        return await _executor.GetAsync<LeagueListDto>(endpoint, cancellationToken);
    }

    public async Task<IApiResult<LeagueDto>> GetLeagueAsync(uint leagueId, CancellationToken cancellationToken = default)
    {
        return await _executor.GetAsync<LeagueDto>($"leagues/{leagueId}", cancellationToken);
    }

    public async Task<IApiResult<BuilderBaseLeagueListDto>> GetBuilderBaseLeaguesAsync(
        uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString("builderbaseleagues", queryParams);

        return await _executor.GetAsync<BuilderBaseLeagueListDto>(endpoint, cancellationToken);
    }

    public async Task<IApiResult<BuilderBaseLeagueDto>> GetBuilderBaseLeagueAsync(uint leagueId, CancellationToken cancellationToken = default)
    {
        return await _executor.GetAsync<BuilderBaseLeagueDto>($"builderbaseleagues/{leagueId}", cancellationToken);
    }

    public async Task<IApiResult<CapitalLeagueListDto>> GetCapitalLeaguesAsync(
        uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString("capitalleagues", queryParams);

        return await _executor.GetAsync<CapitalLeagueListDto>(endpoint, cancellationToken);
    }

    public async Task<IApiResult<CapitalLeagueDto>> GetCapitalLeagueAsync(uint leagueId, CancellationToken cancellationToken = default)
    {
        return await _executor.GetAsync<CapitalLeagueDto>($"capitalleagues/{leagueId}", cancellationToken);
    }

    public async Task<IApiResult<LeagueTierListDto>> GetLeagueTiersAsync(
        uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString("leaguetiers", queryParams);

        return await _executor.GetAsync<LeagueTierListDto>(endpoint, cancellationToken);
    }

    public async Task<IApiResult<LeagueListDto>> GetLeagueTierAsync(uint leagueTierId, CancellationToken cancellationToken = default)
    {
        return await _executor.GetAsync<LeagueListDto>($"leaguetiers/{leagueTierId}", cancellationToken);
    }

    public async Task<IApiResult<WarLeagueListDto>> GetWarLeaguesAsync(
        uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString("warleagues", queryParams);

        return await _executor.GetAsync<WarLeagueListDto>(endpoint, cancellationToken);
    }

    public async Task<IApiResult<WarLeagueDto>> GetWarLeagueAsync(uint leagueId, CancellationToken cancellationToken = default)
    {
        return await _executor.GetAsync<WarLeagueDto>($"warleagues/{leagueId}", cancellationToken);
    }

    /// <param name="leagueId">Now this works only for leagueId = 29000022 (Legend League) </param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task<IApiResult<LeagueSeasonListDto>> GetLeagueSeasonsAsync(uint leagueId = 29000022, CancellationToken cancellationToken = default)
    {
        return await _executor.GetAsync<LeagueSeasonListDto>($"leagues/{leagueId}/seasons", cancellationToken);
    }

    public async Task<IApiResult<WarLeagueListDto>> GetLeagueSeasonRankingsAsync(
        string seasonId, uint leagueId = 29000022, uint? limit = null, string? after = null, string? before = null,
        CancellationToken cancellationToken = default)
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

        return await _executor.GetAsync<WarLeagueListDto>(endpoint, cancellationToken);
    }

    [GeneratedRegex(@"^\d{4}-\d{2}(?:-\d{2})?$", RegexOptions.Compiled)]
    private static partial Regex SeasonIdRegexPattern();
}