using Application.DTOs.Common;

namespace Application.DTOs.Players;

public record PlayerBuilderBaseRankingListDto
{
    public required List<PlayerBuilderBaseRankingDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
}