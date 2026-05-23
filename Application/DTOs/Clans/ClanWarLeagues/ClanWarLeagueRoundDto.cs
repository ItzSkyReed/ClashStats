namespace Application.DTOs.Clans.ClanWarLeagues;

public record ClanWarLeagueRoundDto
{
    public required List<string> WarTags { get; init; }
};