using Domain.Models.Statistics;
using Riok.Mapperly.Abstractions;
using Shared.DTOs.Statistics;

namespace Application.Mappings;

[Mapper]
public static partial class SeasonStatsMapper
{
    [MapperIgnoreSource(nameof(PlayerSeasonStats.PlayerTag))]
    [MapperIgnoreSource(nameof(PlayerSeasonStats.Player))]
    public static partial SeasonStatsDto ToDto(this PlayerSeasonStats clanWarPlayerSummary);

    public static partial IQueryable<SeasonStatsDto> ProjectToDto(this IQueryable<PlayerSeasonStats> query);
}