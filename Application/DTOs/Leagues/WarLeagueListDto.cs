using Application.DTOs.Common;

namespace Application.DTOs.Leagues;

public record WarLeagueListDto
{
    public required List<WarLeagueDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
}