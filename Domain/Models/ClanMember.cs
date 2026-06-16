using Domain.Constants;
using Domain.Models.Analytics.ClanWarLeagues;
using Domain.Models.Analytics.ClanWars;
using Domain.Models.ClanWarLeagues;
using Domain.Models.ClanWars;
using Domain.Models.Statistics;

namespace Domain.Models;

public record ClanMember
{
    public required string Tag { get; set; }
    public int InternalId { get; set; }

    public required string Name { get; set; }
    public short TownHallLevel { get; set; }
    public short BuilderHallLevel { get; set; }


    public short ExpLevel { get; set; }
    public short Trophies { get; set; }
    public int WarStars { get; set; }

    public short BuilderBaseTrophies { get; set; }
    public short BestBuilderBaseTrophies { get; set; }

    public ClanRole? Role { get; set; }
    public int ClanCapitalContributions { get; set; }

    public required bool IsNowInClan { get; set; }
    public WarPreference? WarPreference { get; set; }

    public ICollection<PlayerSeasonStats>? SeasonStats { get; set; } = new List<PlayerSeasonStats>();

    public ICollection<ClanWarPlayerPerformance>? ClanWarPerformances { get; set; } = new List<ClanWarPlayerPerformance>();
    public ICollection<ClanWarLeaguePlayerPerformance>? ClanWarLeaguePerformances { get; set; } = new List<ClanWarLeaguePlayerPerformance>();
    public ICollection<ClanWarLeaguesPlayerSummary>? ClanWarLeaguesPlayerSummaries { get; set; } = new List<ClanWarLeaguesPlayerSummary>();
    public ClanWarPlayerSummary? ClanWarPlayerSummary { get; set; }
    public ICollection<PlayerActivitySnapshot>? ActivitySnapshots { get; set; } = new List<PlayerActivitySnapshot>();
    public PlayerActivityState? ActivityState { get; set; }
}