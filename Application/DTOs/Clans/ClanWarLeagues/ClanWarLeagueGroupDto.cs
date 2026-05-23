namespace Application.DTOs.Clans.ClanWarLeagues;

public record ClanWarLeagueGroupDto
{
    public required string Tag { get; init; }
    public required string State { get; init; }
    public required string Season { get; init; }
    public required List<ClanWarLeagueClanDto> Clans { get; init; }
    public required List<ClanWarLeagueRoundDto> Rounds { get; init; }
}