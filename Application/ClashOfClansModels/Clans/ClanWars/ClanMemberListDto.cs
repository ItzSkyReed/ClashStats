using Application.ClashOfClansModels.Common;

namespace Application.ClashOfClansModels.Clans.ClanWars;

public record ClanMemberListDto
{
    public required List<ClanMemberDto> Items { get; init; }
    public required PagingDto Paging { get; init; }
};