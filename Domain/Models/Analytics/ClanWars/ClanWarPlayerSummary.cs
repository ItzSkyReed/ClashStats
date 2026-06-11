using System.Text.Json.Serialization;

namespace Domain.Models.Analytics.ClanWars;

public record ClanWarPlayerSummary
{
    public string Tag { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public float AverageMapPosition { get; set; }
    public float AverageOpponentMapPosition { get; set; }

    public float AverageTownHallLevel { get; set; }
    public float AverageOpponentTownHallLevel { get; set; }

    /// <summary>
    /// % использованных атак от общего числа (показывает, пропускает ли игрок КВ вообще).
    /// </summary>
    /// <example>
    /// Если за 10 КВ из 20 возможных атак он сделал 18, тут будет 90
    /// </example>
    public float AttackParticipationRate { get; set; }

    /// <summary>
    /// Средний процент разрушения/урона за все проведенные атаки.
    /// </summary>
    /// <example>
    /// 75.3% средний снос.
    /// </example>
    public float AverageDestructionPercentage { get; set; }

    /// <summary>
    /// Среднее количество выбитых звезд за каждую атаку по отдельности, не считаются не сделаенные атаки
    /// </summary>
    /// <example>
    /// 2.33 среднее кол-во звезд.
    /// </example>
    public float AverageStarsPerAttack { get; set; }

    /// <summary>
    /// Среднее количество выбитых звезд за все проведенные атаки.
    /// </summary>
    /// <example>
    /// 5.33 среднее кол-во звезд.
    /// </example>
    public float AverageStarsPerWar { get; set; }

    /// <summary>
    /// % использования именно ПЕРВОЙ атаки в КВ.
    /// </summary>
    public float FirstAttackParticipationRate { get; set; }

    /// <summary>
    /// % использования именно ВТОРОЙ атаки в КВ.
    /// </summary>
    public float SecondAttackParticipationRate { get; set; }

    /// <summary>
    /// % атаки по зеркалу
    /// </summary>
    public float MirrorAttacksRate { get; set; }

    /// <summary>
    /// % трех звезд именно против своего ТХ
    /// </summary>
    public float ThreeStarRateAgainstSameTh { get; set; }

    /// <summary>
    /// Средняя разница между TH оппонента и TH игрока
    /// </summary>
    public float AverageThMismatches { get; set; }

    /// <summary>
    /// История последних 5 КВ (количество атак: 0, 1 или 2).
    /// </summary>
    public IReadOnlyList<short> RecentWarsAttacks { get; set; } = [];

    [JsonIgnore] public ClanMember? ClanMember { get; set; }
}