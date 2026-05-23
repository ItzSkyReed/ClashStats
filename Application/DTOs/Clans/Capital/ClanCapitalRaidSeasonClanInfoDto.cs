namespace Application.DTOs.Clans.Capital;

public record ClanCapitalRaidSeasonClanInfoDto
{
    public required string Tag { get; init; }
    public required string Name { get; init; }
    public required int Level { get; init; }
    public required BadgeUrlsDto BadgeUrls { get; init; }
};