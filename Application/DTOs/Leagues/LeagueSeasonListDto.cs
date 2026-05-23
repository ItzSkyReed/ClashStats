using Application.DTOs.Common;

namespace Application.DTOs.Leagues;

public record LeagueSeasonListDto
{
    public required List<LeagueSeasonDto> Items { get; init; }
    public required PagingDto? Paging { get; init; }
}