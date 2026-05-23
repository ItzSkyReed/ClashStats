using Application.DTOs.Locations;

namespace Application.DTOs.Clans.BuilderBase;

public record ClanBuilderBaseRankingDto
{
    public required int ClanLevel { get; init; }
    public required int ClanBuilderBasePoints { get; init; }
    public required LocationDto Location { get; init; }
    public required int Members { get; init; }

    public required string Tag { get; init; }
    public required string Name { get; init; }
    public required int Rank { get; init; }
    public required int PreviousRank { get; init; }
    public required BadgeUrlsDto BadgeUrls { get; init; }
}