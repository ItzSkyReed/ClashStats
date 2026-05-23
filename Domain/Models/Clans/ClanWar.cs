using Domain.Constants;

namespace Domain.Models.Clans;

public record ClanWar
{
    public int Id { get; set; }
    public ClanWarState? State { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public short TeamSize { get; set; }
    public string? OpponentClanTag { get; set; }
    public string? OpponentClanName { get; set; }
    public short OpponentClanLevel { get; set; }
    public short OpponentAttacks { get; set; }
    public short OurAttacks { get; set; }
    public short OurStars { get; set; }
    public short OpponentStars { get; set; }
    public float OurDestructionPercentage { get; set; }
    public float OpponentDestructionPercentage { get; set; }
    public short? ExpEarned { get; set; }
    public List<ClanWarPlayerPerformance>? PlayersPerformances { get; set; }
}