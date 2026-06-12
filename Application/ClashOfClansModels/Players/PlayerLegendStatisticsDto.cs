using Application.ClashOfClansModels.Leagues;

namespace Application.ClashOfClansModels.Players;

public record PlayerLegendStatisticsDto
{
    public required int LegendTrophies;
    public required LegendLeagueTournamentSeasonResultDto BestSeason { get; init; }
    public required LegendLeagueTournamentSeasonResultDto CurrentSeason { get; init; }
    public required LegendLeagueTournamentSeasonResultDto PreviousSeason { get; init; }
    public required LegendLeagueTournamentSeasonResultDto PreviousBuilderBaseSeason { get; init; }
    public required LegendLeagueTournamentSeasonResultDto BestBuilderBaseSeason { get; init; }
};