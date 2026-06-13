using Shared.DTOs.Statistics;

namespace Shared.DTOs.Summaries;

public record ClanMemberSummaryDto
{
    public string Tag { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public short TownHallLevel { get; set; }
    public short BuilderHallLevel { get; set; }


    public short ExpLevel { get; set; }
    public short Trophies { get; set; }
    public int WarStars { get; set; }

    public short BuilderBaseTrophies { get; set; }
    public short BestBuilderBaseTrophies { get; set; }

    /// <summary>
    /// Роль в клане (notMember, member, leader, coLeader, admin)
    /// </summary>
    public string? Role { get; set; }

    public int ClanCapitalContributions { get; set; }

    public bool IsNowInClan { get; set; }

    /// <summary>
    /// Предпочтение участия в КВ (in, out)
    /// </summary>
    public string WarPreference { get; set; } = string.Empty;

    public required List<SeasonStatsDto> SeasonStats { get; set; }

    public List<ClanWarLeaguesPlayerSummaryDto>? ClanWarLeaguesPlayerSummaries { get; set; }
    public ClanWarPlayerSummaryDto? ClanWarPlayerSummary { get; set; }
}