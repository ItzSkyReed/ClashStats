using Application.ClashOfClansModels.Clans.ClanWars;
using Domain.Constants;

namespace Application.ClashOfClansModels.Clans.ClanWarLeagues;

public record ClanWarLeaguerWarDto
{
    public ClanWarLeagueState? State { get; init; }
    public int TeamSize { get; init; }
    public DateTime PreparationStartTime { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public DateTime WarStartTime { get; init; }
    public required WarClanDto Clan { get; init; }
    public required WarClanDto Opponent { get; init; }
}