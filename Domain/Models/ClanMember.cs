using Domain.Constants;
using Domain.Models.ClanWars;

namespace Domain.Models;

public record ClanMember
{
    public required string Tag { get; set; }
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

    public ICollection<SeasonStats>? SeasonStats { get; set; } = new List<SeasonStats>();

    public ICollection<ClanWarPlayerPerformance>? ClanWarPerformances { get; set; } = new List<ClanWarPlayerPerformance>();
}