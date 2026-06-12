namespace Shared.DTOs.Summaries;

public record ClanWarPlayerSummaryDto
{
    public string Tag { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public float AverageMapPosition { get; set; }
    public float AverageOpponentMapPosition { get; set; }

    public float AverageTownHallLevel { get; set; }
    public float AverageOpponentTownHallLevel { get; set; }

    public float AttackParticipationRate { get; set; }

    public float AverageDestructionPercentage { get; set; }

    public float AverageStarsPerAttack { get; set; }

    public float AverageStarsPerWar { get; set; }


    public float FirstAttackParticipationRate { get; set; }


    public float SecondAttackParticipationRate { get; set; }

    public float MirrorAttacksRate { get; set; }

    public float ThreeStarRateAgainstSameTh { get; set; }

    public float AverageThMismatches { get; set; }

    /// <summary>
    /// История последних 5 КВ (количество атак: 0, 1 или 2).
    /// </summary>
    public IReadOnlyList<short> RecentWarsAttacks { get; set; } = [];
}