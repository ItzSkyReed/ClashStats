using Application.DTOs.Common;

namespace Application.DTOs.Leagues;

public record LeagueTierListDto
{
    public required List<LeagueTierDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
}