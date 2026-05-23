using Application.DTOs.Common;

namespace Application.DTOs.Leagues;

public record LeagueListDto
{
    public required List<LeagueDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
};