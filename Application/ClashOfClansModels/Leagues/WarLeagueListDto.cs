using Application.ClashOfClansModels.Common;

namespace Application.ClashOfClansModels.Leagues;

public record WarLeagueListDto
{
    public required List<WarLeagueDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
}