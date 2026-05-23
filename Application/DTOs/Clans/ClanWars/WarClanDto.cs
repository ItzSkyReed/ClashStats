namespace Application.DTOs.Clans.ClanWars;

public record WarClanDto
{
    public required string Tag{ get; init; }
    public required string Name{ get; init; }
    public required BadgeUrlsDto BadgeUrls{ get; init; }
    public required int ClanLevel{ get; init; }
    public required int Attacks{ get; init; }
    public required int Stars{ get; init; }
    public required float DestructionPercentage{ get; init; }
    public int? ExpEarned{ get; init; }

    public List<ClanWarMemberDto>? Members{ get; init; }
}