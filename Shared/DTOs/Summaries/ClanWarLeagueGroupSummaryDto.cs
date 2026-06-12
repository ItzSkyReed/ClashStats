using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Summaries;

public record ClanWarLeagueGroupSummaryDto
{
    public string Season { get; set; } = string.Empty;

    [AllowedValues("groupNotFound", "preparation", "inWar", "ended")]
    public string State { get; set; } = string.Empty;

    public short TeamSize { get; set; }

    public short? Place { get; set; }

    public short TotalWins { get; set; }
    public short TotalLosses { get; set; }
    public short TotalOurAttacks { get; set; }
    public short TotalOurStars { get; set; }
    public short TotalOurThreeStars { get; set; }

    public short TotalOpponentAttacks { get; set; }
    public short TotalOpponentStars { get; set; }

    public float ParticipationRate { get; set; }

    public float AverageStarsPerAttack { get; set; }

    public float AverageOurDestructionPercentage { get; set; }

    public float AverageOurTownHall { get; set; }

    public float AverageOpponentTownHall { get; set; }

    public float TownHallAdvantage { get; set; }
}