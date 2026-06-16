using System.Text.Json.Serialization;

namespace Domain.Models.Statistics;

public record PlayerSeasonStats
{
    public DateOnly SeasonDate { get; set; }
    public required string PlayerTag { get; set; }
    public int Donations { get; set; }
    public int DonationsReceived { get; set; }

    [JsonIgnore] public ClanMember? Player { get; set; }
}