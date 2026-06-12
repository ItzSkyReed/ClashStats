using Application.ClashOfClansModels.Common;

namespace Application.ClashOfClansModels.Players;

public record PlayerBuilderBaseRankingListDto
{
    public required List<PlayerBuilderBaseRankingDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
}