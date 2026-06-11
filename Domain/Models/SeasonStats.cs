using System.Text.Json.Serialization;

namespace Domain.Models;

public record SeasonStats
{
    public DateOnly SeasonDate { get; set; }
    public required string PlayerTag { get; set; }
    public int Donations { get; set; }
    public int DonationsReceived { get; set; }

    [JsonIgnore] public ClanMember? Player { get; set; }
}