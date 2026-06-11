using System.Text.Json.Serialization;

namespace Domain.Models.ClanWarLeagues;

public record ClanWarLeaguePlayerPerformance
{
    public string WarTag { get; set; } = string.Empty;
    public string PlayerTag { get; set; } = string.Empty;
    public short MapPosition { get; set; }
    public short TownHallLevel { get; set; }

    public short? AttackStars { get; set; }
    public string? DefenderTag { get; set; }
    public short? AttackDestruction { get; set; }
    public short? AttackDuration { get; set; }
    public short? OpponentPosition { get; set; }
    public short? OpponentTownHallLevel { get; set; }

    [JsonIgnore] public ClanWarLeagueWar? LeagueWar { get; set; }

    [JsonIgnore] public ClanMember? Member { get; set; }
}