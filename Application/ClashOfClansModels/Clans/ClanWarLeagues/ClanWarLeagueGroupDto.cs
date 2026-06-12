using Domain.Constants;

namespace Application.ClashOfClansModels.Clans.ClanWarLeagues;

public record ClanWarLeagueGroupDto
{
    public string? Tag { get; set; }
    public required ClanWarLeagueGroupState State { get; init; }
    public required string Season { get; init; }
    public required List<ClanWarLeagueGroupClanDto> Clans { get; init; }
    public required List<ClanWarLeagueRoundDto> Rounds { get; init; }
}