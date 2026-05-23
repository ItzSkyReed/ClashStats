using Application.DTOs.Common;

namespace Application.DTOs.Clans.BuilderBase;

public record ClanBuilderBaseRankingListDto
{
    public required List<ClanBuilderBaseRankingDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
}