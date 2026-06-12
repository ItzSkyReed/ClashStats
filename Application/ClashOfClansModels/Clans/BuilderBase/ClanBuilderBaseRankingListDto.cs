using Application.ClashOfClansModels.Common;

namespace Application.ClashOfClansModels.Clans.BuilderBase;

public record ClanBuilderBaseRankingListDto
{
    public required List<ClanBuilderBaseRankingDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
}