using Application.ClashOfClansModels.Common;
using Application.ClashOfClansModels.Leagues;
using Application.ClashOfClansModels.Locations;

namespace Application.ClashOfClansModels.Clans;

public record ClanListItemDto
{
    public required string Tag { get; init; }
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string Description { get; init; }
    public required bool IsFamilyFriendly { get; init; }

    public required int ClanLevel { get; init; }
    public required int ClanPoints { get; init; }
    public required int ClanBuilderBasePoints { get; init; }
    public required int ClanCapitalPoints { get; init; }

    public required BadgeUrlsDto BadgeUrls { get; init; }
    public required CapitalLeagueDto CapitalLeague { get; init; }
    public required WarLeagueDto WarLeague { get; init; }

    public required int RequiredTrophies { get; init; }
    public required int RequiredBuilderBaseTrophies { get; init; }
    public required int RequiredTownhallLevel { get; init; }


    public required string WarFrequency { get; init; }
    public required int WarWinStreak { get; init; }
    public required int WarWins { get; init; }
    public required int WarTies { get; init; }
    public required int WarLosses { get; init; }
    public required bool IsWarLogPublic { get; init; }
    public required int Members { get; init; }

    public required LocationDto Location { get; init; }
    public required ChatLanguageDto ChatLanguage { get; init; }

    public required List<LabelDto> Labels { get; init; }
}