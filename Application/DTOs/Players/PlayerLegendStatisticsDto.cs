using Application.DTOs.Leagues;

namespace Application.DTOs.Players;

public record PlayerLegendStatisticsDto
{
    public required LegendLeagueTournamentSeasonResultDto BestSeason { get; init; }
    public required LegendLeagueTournamentSeasonResultDto CurrentSeason { get; init; }
    public required LegendLeagueTournamentSeasonResultDto PreviousSeason { get; init; }
    public required LegendLeagueTournamentSeasonResultDto PreviousBuilderBaseSeason { get; init; }
    public required LegendLeagueTournamentSeasonResultDto BestBuilderBaseSeason { get; init; }

    public required int LegendTrophies;
};