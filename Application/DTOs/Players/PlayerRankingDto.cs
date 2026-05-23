using Application.DTOs.Leagues;

namespace Application.DTOs.Players;

public record PlayerRankingDto
{
    public required int AttackWins { get; init; }
    public required int DefenseWins { get; init; }
    public required string Tag { get; init; }
    public required string Name { get; init; }
    public required int ExpLevel { get; init; }
    public required int Rank { get; init; }
    public required int PreviousRank { get; init; }
    public required int Trophies { get; init; }

    public PlayerRankingClanDto? Clan { get; init; }
    public required LeagueTierDto LeagueTier { get; init; }
    public required LeagueDto League { get; init; }
}