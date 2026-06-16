namespace Domain.Models.Statistics;

public class ClanStatsSnapshot
{
    public required string ClanTag { get; set; }
    public DateOnly CapturedAt { get; set; }

    public short ClanLevel { get; set; }
    public int ClanPoints { get; set; }
    public int ClanBuilderBasePoints { get; set; }
    public short ClanCapitalPoints { get; set; }

    public string WarLeagueName { get; set; } = string.Empty;
    public int WarLeagueId { get; set; }

    public string CapitalLeagueName { get; set; } = string.Empty;
    public int CapitalLeagueId { get; set; }

    public short WarWinStreak { get; set; }
    public short WarWins { get; set; }
    public short WarTies { get; set; }
    public short WarLosses { get; set; }

    public short MembersCount { get; set; }
    public float AverageTownHallLevel { get; set; }

    public short RequiredTrophies { get; set; }
    public short RequiredTownHallLevel { get; set; }
}