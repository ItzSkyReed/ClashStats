using Application.ClashOfClansModels.Common;

namespace Application.ClashOfClansModels.Clans.Capital;

public record ClanCapitalRaidSeasonsDto
{
    public required List<ClanCapitalRaidSeasonDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
};