using Application.ClashOfClansModels.Leagues;

namespace Application.ClashOfClansModels.Players;

public record PlayerBuilderBaseRankingDto
{
    public required BuilderBaseLeagueDto BuilderBaseLeague { get; init; }
    public PlayerRankingClanDto? ClanDto { get; init; }
    public required string Tag { get; init; }
    public required string Name { get; init; }
    public required int expLevel { get; init; }
    public required int Rank { get; init; }
    public required int PreviousRank { get; init; }
    public required int BuilderBaseTrophies { get; init; }
}