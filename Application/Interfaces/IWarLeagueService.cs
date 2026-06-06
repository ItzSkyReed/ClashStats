using Application.DTOs.Clans.ClanWarLeagues;

namespace Application.Interfaces;

public interface IWarLeagueService
{
    public ValueTask<short> GetLeagueWarTeamSize(ClanWarLeagueGroupDto leagueGroupDto, CancellationToken ct);

    public Task<List<ClanLeagueRanking>?> CalculateClanWarLeagueRankings(ClanWarLeagueGroupDto leagueGroupDto, CancellationToken ct);

    public short? GetClanPlace(string clanTag, in List<ClanLeagueRanking> rankings);

    public record ClanLeagueRanking(short Place, string Tag, string Name, int TotalStars, float TotalDestruction);
}