using Application.DTOs.Clans;
using Application.DTOs.Clans.BuilderBase;
using Application.DTOs.Clans.Capital;
using Application.DTOs.Clans.ClanWarLeagues;
using Application.DTOs.Clans.ClanWars;
using Application.DTOs.Common;
using Application.DTOs.Leagues;
using Application.DTOs.Locations;
using Application.DTOs.Players;
using Domain.Constants;

namespace Application.Interfaces;

public interface IClashApiClient
{
    public Task<IApiResult<ClanDto>> GetClanAsync(string clanTag, CancellationToken cancellationToken = default);

    public Task<IApiResult<ClanMemberListDto>> GetClanMembersAsync(
        string clanTag, uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default);

    public Task<IApiResult<ClanCapitalRaidSeasonsDto>> GetClanCapitalRaidSeasonsAsync(
        string clanTag, uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default);

    public Task<IApiResult<ClanWarLogDto>> GetClanWarLogAsync(
        string clanTag, uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default);

    public Task<IApiResult<ClanWarDto>> GetCurrentClanWarAsync(string clanTag, CancellationToken cancellationToken = default);

    public Task<IApiResult<ClanListDto>> GetClansAsync(
        string? name = null, WarFrequency? warFrequency = null, uint? limit = null, uint? locationId = null,
        string? after = null, string? before = null, uint? maxMembers = null, uint? minMembers = null,
        uint? minClanPoints = null, uint? minClanLevel = null, List<uint>? labelIds = null, CancellationToken cancellationToken = default);

    public Task<IApiResult<ClanWarLeagueGroupDto>> GetCurrentClanWarLeagueGroupAsync(string clanTag, CancellationToken cancellationToken = default);

    public Task<IApiResult<ClanWarLeagueGroupDto>> GetClanWarLeagueWarAsync(string warTag, CancellationToken cancellationToken = default);

    public Task<IApiResult<LabelListDto>> GetPlayerLabelsAsync(uint? limit = null, string? after = null, string? before = null,
        CancellationToken cancellationToken = default);

    public Task<IApiResult<LabelListDto>> GetClanLabelsAsync(uint? limit = null, string? after = null, string? before = null,
        CancellationToken cancellationToken = default);

    public Task<IApiResult<LeagueListDto>> GetLeaguesAsync(uint? limit = null, string? after = null, string? before = null,
        CancellationToken cancellationToken = default);

    public Task<IApiResult<LeagueDto>> GetLeagueAsync(uint leagueId, CancellationToken cancellationToken = default);

    public Task<IApiResult<BuilderBaseLeagueListDto>> GetBuilderBaseLeaguesAsync(uint? limit = null, string? after = null, string? before = null,
        CancellationToken cancellationToken = default);

    public Task<IApiResult<BuilderBaseLeagueDto>> GetBuilderBaseLeagueAsync(uint leagueId, CancellationToken cancellationToken = default);

    public Task<IApiResult<CapitalLeagueListDto>> GetCapitalLeaguesAsync(uint? limit = null, string? after = null, string? before = null,
        CancellationToken cancellationToken = default);

    public Task<IApiResult<CapitalLeagueDto>> GetCapitalLeagueAsync(uint leagueId, CancellationToken cancellationToken = default);

    public Task<IApiResult<LeagueTierListDto>> GetLeagueTiersAsync(uint? limit = null, string? after = null, string? before = null,
        CancellationToken cancellationToken = default);

    public Task<IApiResult<LeagueListDto>> GetLeagueTierAsync(uint leagueTierId, CancellationToken cancellationToken = default);

    public Task<IApiResult<WarLeagueListDto>> GetWarLeaguesAsync(uint? limit = null, string? after = null, string? before = null,
        CancellationToken cancellationToken = default);

    public Task<IApiResult<WarLeagueDto>> GetWarLeagueAsync(uint leagueId, CancellationToken cancellationToken = default);

    public Task<IApiResult<LeagueSeasonListDto>> GetLeagueSeasonsAsync(uint leagueId = 29000022, CancellationToken cancellationToken = default);

    public Task<IApiResult<WarLeagueListDto>> GetLeagueSeasonRankingsAsync(
        string seasonId, uint leagueId = 29000022, uint? limit = null, string? after = null, string? before = null,
        CancellationToken cancellationToken = default);

    public Task<IApiResult<LocationListDto>> GetLocationAsync(int locationId, CancellationToken cancellationToken = default);

    public Task<IApiResult<LocationListDto>> GetLocationsAsync(uint? limit = null, string? after = null, string? before = null,
        CancellationToken cancellationToken = default);

    public Task<IApiResult<ClanCapitalRankingListDto>> GetCapitalRankingsByLocationAsync(
        uint locationId, uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default);

    public Task<IApiResult<ClanBuilderBaseRankingListDto>> GetClanBuilderBaseRankingsByLocationAsync(
        uint locationId, uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default);

    public Task<IApiResult<PlayerBuilderBaseRankingListDto>> GetPlayerBuilderBaseRankingsByLocationAsync(
        uint locationId, uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default);

    public Task<IApiResult<PlayerBuilderBaseRankingListDto>> GetPlayerRankingsByLocationAsync(
        uint locationId, uint? limit = null, string? after = null, string? before = null, CancellationToken cancellationToken = default);

    public Task<IApiResult<PlayerDto>> GetPlayerAsync(string playerTag, CancellationToken cancellationToken = default);

    public Task<IApiResult<VerifyTokenResponseDto>> PostVerifyTokenAsync(string playerTag, string token,
        CancellationToken cancellationToken = default);
}