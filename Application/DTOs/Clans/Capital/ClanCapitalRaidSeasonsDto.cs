using Application.DTOs.Common;

namespace Application.DTOs.Clans.Capital;

public record ClanCapitalRaidSeasonsDto
{
    public required List<ClanCapitalRaidSeasonDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
};