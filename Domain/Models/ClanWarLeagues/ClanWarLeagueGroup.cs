using Domain.Constants;

namespace Domain.Models.ClanWarLeagues;

public record ClanWarLeagueGroup
{
    public required string Season { get; set; }
    public required ClanWarLeagueGroupState State { get; set; }
    public short TeamSize { get; set; }
    public short? Place { get; set; }

    public List<ClanWarLeagueWar>? Wars { get; set; }
}