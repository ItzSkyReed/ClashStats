using Application.DTOs.Common;

namespace Application.DTOs.Leagues;

public record LeagueDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required IconUrlsDto IconUrls { get; init; }
}