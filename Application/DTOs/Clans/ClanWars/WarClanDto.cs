namespace Application.DTOs.Clans.ClanWars;

public record WarClanDto
{
    public string? Tag { get; init; }
    public string? Name { get; init; }
    public required BadgeUrlsDto BadgeUrls { get; init; }
    public required int ClanLevel { get; init; }
    public int? Attacks { get; init; }
    public required int Stars { get; init; }
    public required float DestructionPercentage { get; init; }
    public int? ExpEarned { get; init; }

    public List<ClanWarMemberDto>? Members { get; init; }
}