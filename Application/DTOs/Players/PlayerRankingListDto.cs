using Application.DTOs.Common;

namespace Application.DTOs.Players;

public record PlayerRankingListDto
{
    public required List<PlayerRankingDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
}