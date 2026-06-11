using System.Text.Json.Serialization;

namespace Domain.Models.Analytics.ClanWarLeagues;

public record ClanWarLeaguesPlayerSummary
{
    public string Season { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public float AverageMapPosition { get; set; }
    public float AverageOpponentMapPosition { get; set; }
    public float AverageTownHallLevel { get; set; }
    public float AverageOpponentTownHallLevel { get; set; }

    /// <summary>
    /// Средний дисбаланс по Ратушам (OpponentTH - PlayerTH).
    /// Положительное число означает, что игрок чаще атаковал ТХ выше своего,
    /// отрицательное — атаковал более слабых.
    /// </summary>
    public float AverageThMismatches { get; set; }

    /// <summary>
    /// Сколько всего раундов (дней) игрок физически присутствовал на карте (был выставлен в бой).
    /// Максимум 7 за одну неделю ЛВК.
    /// </summary>
    public int TotalRoundsOnMap { get; set; }

    /// <summary>
    /// % выполненных атак от количества раундов, когда игрок БЫЛ на карте.
    /// Показывает дисциплину: если его поставили 4 раза, а он атаковал 3 — тут будет 75%.
    /// </summary>
    public float AttackParticipationRate { get; set; }

    public float AverageDestructionPercentage { get; set; }

    /// <summary>
    /// Среднее количество звезд за ОДНУ фактически сделанную атаку (от 0.00 до 3.00).
    /// </summary>
    public float AverageStarsPerAttack { get; set; }

    /// <summary>
    /// % трех звезд строго против своего уровня ТХ (TownHallLevel == OpponentTownHallLevel).
    /// </summary>
    public float ThreeStarRateAgainstSameTh { get; set; }

    /// <summary>
    /// % атак по зеркалу (совпадение порядкового номера на карте). .
    /// </summary>
    public float MirrorAttacksRate { get; set; }

    /// <summary>
    /// Результаты последних 7 раундов ЛВК.
    /// Значения:
    /// -2 = игрок не был в клане/заявке
    /// -1 = сидел на замене в этот день (не был на карте)
    ///  0 = проспал атаку (был на карте, но не напал)
    ///  1, 2, 3 = количество взятых звезд
    /// </summary>
    /// <example> На фронте рисуется красивой строкой из 7 цветных кружков: [3, 2, -1, -1, 3, 0, 1] </example>
    public IReadOnlyList<short> CwlRoundsAttacks { get; set; } = [];

    [JsonIgnore] public ClanMember? ClanMember { get; set; }
}