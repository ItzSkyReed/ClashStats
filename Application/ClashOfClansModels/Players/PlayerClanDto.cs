using Application.ClashOfClansModels.Clans;

namespace Application.ClashOfClansModels.Players;

public record PlayerClanDto
{
    public required string Tag { get; init; }
    public required string Name { get; init; }
    public required int ClanLevel { get; init; }
    public required BadgeUrlsDto BadgeUrls { get; init; }
}