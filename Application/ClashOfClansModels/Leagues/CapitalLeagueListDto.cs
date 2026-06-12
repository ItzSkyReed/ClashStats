using Application.ClashOfClansModels.Common;

namespace Application.ClashOfClansModels.Leagues;

public record CapitalLeagueListDto
{
    public required List<CapitalLeagueDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
}