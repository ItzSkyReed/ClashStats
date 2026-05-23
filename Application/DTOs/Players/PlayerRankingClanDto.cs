using Application.DTOs.Clans;

namespace Application.DTOs.Players;

public record PlayerRankingClanDto
{
    public required string Tag { get; init; }
    public required string Name { get; init; }
    public required BadgeUrlsDto BadgeUrlsDto { get; init; }
}