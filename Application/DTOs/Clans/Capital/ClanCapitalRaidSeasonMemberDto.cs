namespace Application.DTOs.Clans.Capital;

public record ClanCapitalRaidSeasonMemberDto
{
    public required string Tag { get; init; }
    public required string Name { get; init; }
    public required int Attacks { get; init; }
    public required int AttackLimit { get; init; }
    public required int BonusAttackLimit { get; init; }
    public required int CapitalResourcesLooted { get; init; }
};