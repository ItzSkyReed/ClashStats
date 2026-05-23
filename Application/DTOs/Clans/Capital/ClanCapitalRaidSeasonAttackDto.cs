namespace Application.DTOs.Clans.Capital;

public record ClanCapitalRaidSeasonAttackDto
{
    public required ClanCapitalRaidSeasonAttackerDto Attacker { get; init; }
    public required int DestructionPercent { get; init; }
    public required int Stars { get; init; }
};