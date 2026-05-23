using Application.DTOs.Common;
using Application.DTOs.Leagues;
using Application.DTOs.Players;
using Domain.Constants;

namespace Application.DTOs.Clans;

public record ClanMemberDto
{
    public required string Tag { get; init; }
    public required string Name { get; init; }
    public required ClanRole Role { get; init; }
    public required int TownHallLevel { get; init; }
    public required int ExpLevel { get; init; }
    public required LeagueDto League { get; init; }
    public required LeagueTierDto LeagueTier { get; init; }
    public required int Trophies { get; init; }
    public required int BuilderBaseTrophies { get; init; }
    public required int ClanRank { get; init; }
    public required int PreviousClanRank { get; init; }
    public required int Donations { get; init; }
    public required int DonationsReceived { get; init; }
    public PagingDto? Paging { get; init; }
}