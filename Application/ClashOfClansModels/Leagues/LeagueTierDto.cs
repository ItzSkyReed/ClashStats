using Application.ClashOfClansModels.Common;

namespace Application.ClashOfClansModels.Leagues;

public record LeagueTierDto
{
    public required string Name { get; init; }
    public required int Id { get; init; }
    public required IconUrlsDto IconUrls { get; init; }
};