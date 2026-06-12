using Application.ClashOfClansModels.Common;

namespace Application.ClashOfClansModels.Leagues;

public record LeagueTierListDto
{
    public required List<LeagueTierDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
}