using Application.ClashOfClansModels.Common;

namespace Application.ClashOfClansModels.Leagues;

public record LeagueSeasonListDto
{
    public required List<LeagueSeasonDto> Items { get; init; }
    public required PagingDto? Paging { get; init; }
}