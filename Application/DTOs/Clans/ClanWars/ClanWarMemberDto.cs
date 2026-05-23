namespace Application.DTOs.Clans.ClanWars;

public record ClanWarMemberDto
{
    public required string Tag { get; init; }
    public required string Name { get; init; }
    public required int TownhallLevel { get; init; }
    public required int MapPosition { get; init; }
    public required int OpponentAttacks { get; init; }
    public ClanWarAttackDto? BestOpponentAttack { get; init; }
    public List<ClanWarAttackDto>? Attacks { get; init; }
}