namespace Application.DTOs.Clans.ClanWarLeagues;

public record ClanWarLeagueGroupClanDto
{
    public required string Tag { get; init; }
    public required int ClanLevel { get; init; }
    public required string Name { get; init; }
    public required List<ClanWarLeagueClanMemberDto> Members { get; init; }
    public required BadgeUrlsDto BadgeUrls { get; init; }
};