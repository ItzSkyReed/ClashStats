using Domain.Constants;

namespace Domain.Models.Analytics.ClanWarLeagues;

public record ClanWarLeagueGroupSummary
{
    /// <summary>
    /// Сезон (например, "2026-05").
    /// </summary>
    public string Season { get; set; } = string.Empty;

    public ClanWarLeagueState State { get; set; }
    public short TeamSize { get; set; }

    /// <summary>
    /// Итоговое место клана в группе (от 1 до 8).
    /// </summary>
    public short? Place { get; set; }

    /// <summary>
    /// Количество выигранных войн за сезон.
    /// </summary>
    public short TotalWins { get; set; }

    /// <summary>
    /// Количество проигранных войн за сезон.
    /// </summary>
    public short TotalLosses { get; set; }


    public short TotalOurAttacks { get; set; }
    public short TotalOurStars { get; set; }

    /// <summary>
    /// Сколько всего 3-звездочных атак сделал клан за сезон.
    /// </summary>
    public short TotalOurThreeStars { get; set; }


    public short TotalOpponentAttacks { get; set; }
    public short TotalOpponentStars { get; set; }

    /// <summary>
    /// % использованных атак кланом за всю неделю.
    /// Расчет: TotalOurAttacks / (RoundsCompleted * TeamSize) * 100
    /// </summary>
    public float ParticipationRate { get; set; }

    /// <summary>
    /// Среднее количество звезд за одну атаку  клана в этом сезоне ЛВК.
    /// </summary>
    public float AverageStarsPerAttack { get; set; }

    /// <summary>
    /// Средний процент разрушения за одну войну.
    /// </summary>
    public float AverageOurDestructionPercentage { get; set; }

    /// <summary>
    /// Средний уровень ТХ нашего клана на карте за все 7 дней.
    /// </summary>
    public float AverageOurTownHall { get; set; }

    /// <summary>
    /// Средний уровень ТХ всех оппонентов за 7 дней.
    /// </summary>
    public float AverageOpponentTownHall { get; set; }

    /// <summary>
    /// Разница между нашим средним ТХ и средним ТХ оппонентов (AverageOur - AverageOpponent).
    /// Показывает, насколько клану "не повезло" с подбором лиги.
    /// </summary>
    public float TownHallAdvantage { get; set; }
}