using Application.DTOs.Common;

namespace Application.DTOs.Clans.Capital;

public record ClanCapitalRankingListDto
{
    public required List<ClanCapitalRankingDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
}