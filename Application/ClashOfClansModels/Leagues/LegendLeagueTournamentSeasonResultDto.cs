namespace Application.ClashOfClansModels.Leagues;

public record LegendLeagueTournamentSeasonResultDto
{
    public required int Trohies { get; init; }
    public required int Id { get; init; }
    public required int Rank { get; init; }
};