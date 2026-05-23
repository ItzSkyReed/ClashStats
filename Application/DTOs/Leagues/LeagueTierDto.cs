using Application.DTOs.Common;

namespace Application.DTOs.Leagues;

public record LeagueTierDto
{
    public required string Name { get; init; }
    public required int Id { get; init; }
    public required IconUrlsDto IconUrls { get; init; }
};