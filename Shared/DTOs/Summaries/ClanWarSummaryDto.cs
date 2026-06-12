namespace Shared.DTOs.Summaries;

public record ClanWarSummaryDto
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public short TeamSize { get; set; }
    public string OpponentClanName { get; set; } = string.Empty;
    public short OpponentClanLevel { get; set; }
    public short OpponentAttacks { get; set; }
    public short OurAttacks { get; set; }
    public short OurStars { get; set; }
    public short OpponentStars { get; set; }
    public float OurDestructionPercentage { get; set; }
    public float OpponentDestructionPercentage { get; set; }
    public short ExpEarned { get; set; }

    /// <summary>
    /// Результат войны (win, lose, tie).
    /// </summary>
    public string Result { get; set; } = string.Empty;

    public float AverageOurTownHall { get; set; }
    public float AverageOpponentTownHall { get; set; }
    public float TownHallAdvantage { get; set; }
    public float ParticipationRate { get; set; }
    public short OurThreeStarsCount { get; set; }
    public float StarsPerAttack { get; set; }
}