using Application.DTOs.Common;

namespace Application.DTOs.Clans.ClanWars;

public record ClanWarLogDto
{
    public required List<ClanWarLogEntryDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
}