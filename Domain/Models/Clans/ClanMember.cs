using Domain.Constants;

namespace Domain.Models.Clans;

// public record PlayerDto
// {
//
//
//
//
//     public required LeagueDto League { get; init; }
//     public required LeagueTierDto LeagueTier { get; init; }
//     public required BuilderBaseLeagueDto BuilderBaseLeague { get; init; }
//
//     public required string Role { get; init; }
//     public required string WarPreference { get; init; }
//
//     public required int AttackWins { get; init; }
//     public required int DefenseWins { get; init; }
//
//     public required List<LabelDto> Labels { get; init; }
// }

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