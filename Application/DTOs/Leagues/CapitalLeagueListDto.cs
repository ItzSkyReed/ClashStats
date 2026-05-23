using Application.DTOs.Common;

namespace Application.DTOs.Leagues;

public record CapitalLeagueListDto
{
    public required List<CapitalLeagueDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
}