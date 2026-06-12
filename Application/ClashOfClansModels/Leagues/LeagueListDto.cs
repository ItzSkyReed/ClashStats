using Application.ClashOfClansModels.Common;

namespace Application.ClashOfClansModels.Leagues;

public record LeagueListDto
{
    public required List<LeagueDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
};