namespace Application.DTOs.Clans.Capital;

public record ClanCapitalRaidSeasonDto
{
    public required List<ClanCapitalRaidSeasonAttackLogEntryDto> AttackLog { get; init; }
    public required List<ClanCapitalRaidSeasonDefenseLogEntryDto> DefenseLog { get; init; }
    public required string State { get; init; }
    public required DateTime StartTime { get; init; }
    public required DateTime EndTime { get; init; }
    public required int CapitalTotalLoot { get; init; }
    public required int RaidsCompleted { get; init; }
    public required int TotalAttacks { get; init; }
    public required int EnemyDistrictsDestroyed { get; init; }
    public required int OffensiveReward { get; init; }
    public required int DefensiveReward { get; init; }
    public required List<ClanCapitalRaidSeasonMemberDto> Members { get; init; }
};