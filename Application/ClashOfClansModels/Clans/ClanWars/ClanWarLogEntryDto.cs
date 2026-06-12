namespace Application.ClashOfClansModels.Clans.ClanWars;

public record ClanWarLogEntryDto
{
    public required int TeamSize { get; init; }
    public required string Result { get; init; }
    public required DateTime EndTime { get; init; }
    public required int AttacksPerMember { get; init; }
    public required string BattleModifier { get; init; }
    public required WarClanDto Clan { get; init; }
    public required WarClanDto Opponent { get; init; }
};