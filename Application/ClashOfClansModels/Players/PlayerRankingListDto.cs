using Application.ClashOfClansModels.Common;

namespace Application.ClashOfClansModels.Players;

public record PlayerRankingListDto
{
    public required List<PlayerRankingDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
}