using Application.ClashOfClansModels.Common;

namespace Application.ClashOfClansModels.Clans;

public record ClanListDto
{
    public required List<ClanDto> Items { get; init; }
    public PagingDto? Paging { get; init; }
};