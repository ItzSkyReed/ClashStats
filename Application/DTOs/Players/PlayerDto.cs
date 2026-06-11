using Application.DTOs.Common;
using Application.DTOs.Leagues;
using Domain.Constants;

namespace Application.DTOs.Players;

public record PlayerDto
{
    public required string Tag { get; init; }
    public required string Name { get; init; }
    public required int ExpLevel { get; init; }
    public required int Trophies { get; init; }
    public required int BestTrophies { get; init; }
    public required int Donations { get; init; }
    public required int DonationsReceived { get; init; }
    public required int BuilderHallLevel { get; init; }
    public required int BuilderBaseTrophies { get; init; }
    public required int BestBuilderBaseTrophies { get; init; }
    public required int WarStars { get; init; }
    public required int ClanCapitalContributions { get; init; }
    public required int CurrentLeagueSeasonId { get; init; }

    public required List<PlayerAchievementProgressDto> Achievements { get; init; }
    public required PlayerHouseDto PlayerHouse { get; init; }

    public required LeagueTierDto LeagueTier { get; init; }
    public required BuilderBaseLeagueDto BuilderBaseLeague { get; init; }

    public PlayerClanDto? Clan { get; init; }
    public ClanRole? Role { get; init; }
    public WarPreference? WarPreference { get; init; }

    public required int AttackWins { get; init; }
    public required int DefenseWins { get; init; }

    public required int TownHallLevel { get; init; }
    public required int TownHallWeaponLevel { get; init; }


    public required List<PlayerItemLevelDto> Troops { get; init; }
    public required List<PlayerItemLevelDto> Heroes { get; init; }
    public required List<PlayerItemLevelDto> Spells { get; init; }

    public required List<LabelDto> Labels { get; init; }
}