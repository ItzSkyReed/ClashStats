using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Summaries;

public record ClanWarLeaguesPlayerSummaryDto
{
    public string Season { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public float AverageMapPosition { get; set; }
    public float AverageOpponentMapPosition { get; set; }
    public float AverageTownHallLevel { get; set; }
    public float AverageOpponentTownHallLevel { get; set; }

    public float AverageThMismatches { get; set; }

    public int TotalRoundsOnMap { get; set; }

    public float AttackParticipationRate { get; set; }

    public float AverageDestructionPercentage { get; set; }

    public float AverageStarsPerAttack { get; set; }

    public float ThreeStarRateAgainstSameTh { get; set; }

    public float MirrorAttacksRate { get; set; }

    /// <summary>
    /// Результаты последних 7 раундов ЛВК. Значения:
    /// -2 = игрок не был в клане/заявке
    /// -1 = сидел на замене в этот день (не был на карте)
    /// 0 = проспал атаку (был на карте, но не напал или 0 звезд)
    /// 1, 2, 3 = количество взятых звезд
    /// </summary>
    /// <example>
    /// [3, 2, -1, -1, 3, 0, 1]
    /// </example>
    [AllowedValues((short)-2, (short)-1, (short)0, (short)1, (short)2, (short)3)]
    public IReadOnlyList<short> CwlRoundsAttacks { get; set; } = [];
}