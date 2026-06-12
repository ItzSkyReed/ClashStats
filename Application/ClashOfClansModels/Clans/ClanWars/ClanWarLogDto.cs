using Application.ClashOfClansModels.Common;

namespace Application.ClashOfClansModels.Clans.ClanWars;

public record ClanWarLogDto
{
    public required List<ClanWarLogEntryDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
}