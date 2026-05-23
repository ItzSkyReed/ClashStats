using Application.DTOs.Common;

namespace Application.DTOs.Clans.ClanWars;

public record ClanMemberListDto
{
    public required List<ClanMemberDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
};