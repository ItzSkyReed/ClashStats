using Domain.Constants;

namespace Domain.Models.ClanWarLeagues;

public record ClanWarLeagueWar
{
    public required string WarTag { get; set; }
    public required ClanWarLeagueState State { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime WarStartTime { get; set; }
    public required string Season { get; set; }
    public short Round { get; set; }
    public string OpponentClanTag { get; set; } = string.Empty;
    public string OpponentClanName { get; set; } = string.Empty;
    public short OpponentClanLevel { get; set; }

    public short OpponentAttacks { get; set; }
    public short OpponentStars { get; set; }
    public float OpponentDestructionPercentage { get; set; }

    public short OurAttacks { get; set; }
    public short OurStars { get; set; }
    public float OurDestructionPercentage { get; set; }

    public List<ClanWarLeaguePlayerPerformance>? PlayerPerformances { get; set; }
    public ClanWarLeagueGroup? Group { get; set; }
}