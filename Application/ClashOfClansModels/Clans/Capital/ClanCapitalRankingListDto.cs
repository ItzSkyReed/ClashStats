using Application.ClashOfClansModels.Common;

namespace Application.ClashOfClansModels.Clans.Capital;

public record ClanCapitalRankingListDto
{
    public required List<ClanCapitalRankingDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
}