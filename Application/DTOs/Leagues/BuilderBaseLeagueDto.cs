namespace Application.DTOs.Leagues;

public record BuilderBaseLeagueDto
{
    public required string Name { get; init; }
    public required int Id { get; init; }
};