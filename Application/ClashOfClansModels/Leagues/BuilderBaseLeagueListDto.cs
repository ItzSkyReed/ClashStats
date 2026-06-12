using Application.ClashOfClansModels.Common;

namespace Application.ClashOfClansModels.Leagues;

public record BuilderBaseLeagueListDto
{
    public required List<BuilderBaseLeagueDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
}