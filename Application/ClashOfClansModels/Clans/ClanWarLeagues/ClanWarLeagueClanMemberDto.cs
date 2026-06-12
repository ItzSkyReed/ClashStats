namespace Application.ClashOfClansModels.Clans.ClanWarLeagues;

public record ClanWarLeagueClanMemberDto
{
    public required string Tag { get; init; }
    public required int TownHallLevel { get; init; }
    public required string Name { get; init; }
};