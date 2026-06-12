namespace Application.ClashOfClansModels.Clans.Capital;

public record ClanCapitalRaidSeasonAttackLogEntryDto
{
    public required ClanCapitalRaidSeasonClanInfoDto Defender { get; init; }
    public required int AttackCount { get; init; }
    public required int DistrictCount { get; init; }
    public required int DistrictsDestroyed { get; init; }
    public required List<ClanCapitalRaidSeasonDistrictDto> Districts { get; init; }
};