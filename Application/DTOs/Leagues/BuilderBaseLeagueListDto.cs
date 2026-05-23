using Application.DTOs.Common;

namespace Application.DTOs.Leagues;

public record BuilderBaseLeagueListDto
{
    public required List<BuilderBaseLeagueDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
}