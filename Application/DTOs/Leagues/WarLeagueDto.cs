namespace Application.DTOs.Leagues;

public record WarLeagueDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}