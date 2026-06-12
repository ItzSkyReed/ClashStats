using Application.ClashOfClansModels.Locations;

namespace Application.ClashOfClansModels.Clans;

public record ClanRankingDto
{
    public required int ClanLevel { get; init; }
    public required int ClanPoints { get; init; }
    public required LocationDto Location { get; init; }
    public required int Members { get; init; }

    public required string Tag { get; init; }
    public required string Name { get; init; }
    public required int Rank { get; init; }
    public required int PreviousRank { get; init; }
    public required BadgeUrlsDto BadgeUrls { get; init; }
}