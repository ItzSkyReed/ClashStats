using Domain.Constants;

namespace Application.DTOs.Clans.ClanWarLeagues;

public record ClanWarLeagueGroupDto
{
    public string? Tag { get; set; }
    public required ClanWarLeagueState State { get; init; }
    public required string Season { get; init; }
    public required List<ClanWarLeagueGroupClanDto> Clans { get; init; }
    public required List<ClanWarLeagueRoundDto> Rounds { get; init; }
}