using Application.DTOs.Common;
using Domain.Constants;

namespace Application.DTOs.Clans.ClanWars;

public record ClanWarDto
{
    public required ClanWarState State{ get; init; }
    public required int TeamSize{ get; init; }
    public required int AttacksPerMember{ get; init; }
    public required string BattleModifier{ get; init; }
    public required DateTime PreparationStartTime{ get; init; }
    public required DateTime StartTime{ get; init; }
    public required DateTime EndTime{ get; init; }
    public required WarClanDto Clan{ get; init; }
    public WarClanDto? Opponent{ get; init; }
    public PagingDto? Paging { get; init; }
}