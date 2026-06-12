namespace Shared.DTOs.Statistics;

public record SeasonStatsDto
{
    public DateOnly SeasonDate { get; set; }
    public int Donations { get; set; }
    public int DonationsReceived { get; set; }
}