namespace Application.DTOs.Leagues;

public record CapitalLeagueDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}