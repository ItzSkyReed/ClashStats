namespace Application.DTOs.Clans.Capital;

public record ClanCapitalRaidSeasonDistrictDto
{
    public required int Stars { get; init; }
    public required string Name { get; init; }
    public required int Id { get; init; }
    public required int DestructionPercent { get; init; }
    public required int AttackCount { get; init; }
    public required int TotalLooted { get; init; }
    public required List<ClanCapitalRaidSeasonAttackDto> Attacks { get; init; }
    public required int DistrictHallLevel { get; init; }
}