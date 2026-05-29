using Application.DTOs.Common;
using Application.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace Infrastructure.Api;

public partial class ClashApiClient
{
    public async Task<IApiResult<LabelListDto>> GetPlayerLabelsAsync(
        uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString("labels/players", queryParams);

        return await _executor.GetAsync<LabelListDto>(endpoint, cancellationToken);
    }

    public async Task<IApiResult<LabelListDto>> GetClanLabelsAsync(
        uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default)
    {
        ValidateExclusiveBeforeAfter(before, after);

        var queryParams = new Dictionary<string, string?>();

        AddQueryParam(queryParams, "limit", limit);
        AddQueryParam(queryParams, "after", after);
        AddQueryParam(queryParams, "before", before);

        var endpoint = QueryHelpers.AddQueryString("labels/clans", queryParams);

        return await _executor.GetAsync<LabelListDto>(endpoint, cancellationToken);
    }
}