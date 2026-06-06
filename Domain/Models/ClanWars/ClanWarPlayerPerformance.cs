namespace Domain.Models.ClanWars;

public record ClanWarPlayerPerformance
{
    public int WarId { get; set; }
    public string PlayerTag { get; set; } = string.Empty;
    public short MapPosition { get; set; }
    public short TownHallLevel { get; set; }

    public short? Attack1Stars { get; set; }
    public string? Defender1Tag { get; set; }
    public short? Attack1Destruction { get; set; }
    public short? Attack1Duration { get; set; }
    public short? Opponent1Position { get; set; }
    public short? Opponent1TownHallLevel { get; set; }

    public short? Attack2Stars { get; set; }
    public string? Defender2Tag { get; set; }
    public short? Attack2Duration { get; set; }
    public short? Attack2Destruction { get; set; }
    public short? Opponent2Position { get; set; }
    public short? Opponent2TownHallLevel { get; set; }

    public ClanWar? War { get; set; }
    public ClanMember? Member { get; set; }
}