namespace Application.DTOs.Clans.Capital;

public record ClanCapitalRaidSeasonDefenseLogEntryDto
{
    public required ClanCapitalRaidSeasonClanInfoDto Attacker { get; init; }
    public required int AttackCount { get; init; }
    public required int DistrictCount { get; init; }
    public required int DistrictsDestroyed { get; init; }
    public required List<ClanCapitalRaidSeasonDistrictDto> Districts { get; init; }
};