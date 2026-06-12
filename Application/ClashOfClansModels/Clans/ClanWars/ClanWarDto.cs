using Application.ClashOfClansModels.Common;
using Domain.Constants;

namespace Application.ClashOfClansModels.Clans.ClanWars;

public record ClanWarDto
{
    public required ClanWarState State { get; init; }
    public int? TeamSize { get; init; }
    public int? AttacksPerMember { get; init; }
    public string? BattleModifier { get; init; }
    public DateTime PreparationStartTime { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public required WarClanDto Clan { get; init; }
    public WarClanDto? Opponent { get; init; }
    public PagingDto? Paging { get; init; }
}